using System;
using System.Collections.Generic;
using System.Linq;

namespace Neurones
{
    public class Network
    {
        public Network(Layer lastLayer, IEnumerable<Error> expectedOutput)
        {
            this.lastLayer = lastLayer;
            this.expectedOutput = expectedOutput;
        }

        private Layer lastLayer;
        private IEnumerable<Error> expectedOutput;

        public IEnumerable<Error> errors()
        {
            return this.expectedOutput.Select(o => 
                new ExitError(
                    o.neuroneIndex(),
                    new Substr(o.asNumber(), lastLayer.outputValue(o.neuroneIndex()))
                )
            );
        }
    }

    public interface Neurone
    {
        Number outputValue(Layer prevLayer);
        bool find(int targetNeurone);      
        IEnumerable<Synapse> synapsesFrom();

        Error error(IEnumerable<Error> nextErrors, IEnumerable<Synapse> synapses);
        Neurone withNewSynapse(IEnumerable<Error> nextErrors);
        Neurone withValue(Layer prevLayer);
    }

    public class InputNeurone : Neurone
    {
        public InputNeurone(int index, double value)
		{
			this.index = index;
            this.value = new DefaultNumber(value);
		}

        private int index;
        private Number value;

        public Number outputValue(Layer prevLayer)
        {
            return value;
        }

        public bool find(int targetNeurone)
        {
            return targetNeurone == this.index;
        }

        public IEnumerable<Synapse> synapsesFrom()
        {
            throw new Exception("INPUT NEURONE DOES NOT HAVE INPUT SYNAPSE");
        }

        public Error error(IEnumerable<Error> nextErrors, IEnumerable<Synapse> synapses)
        {
            throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
        }

        public Neurone withNewSynapse(IEnumerable<Error> nextErrors)
        {
            throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
        }

        public Neurone withValue(Layer layer)
        {
            throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
        }

    }

    public class DeepNeurone : Neurone
    {

        public DeepNeurone(int index, params Synapse[] synapses)
		{
			this.index = index;
			this.synapses = synapses;
            this.value = new NullNumber();
		}

        public DeepNeurone(int index, Number value, params Synapse[] synapses)
        {
            this.index = index;
            this.synapses = synapses;
            this.value = value;
        }

		private int index;
		private IEnumerable<Synapse> synapses;
        private Number value;

        public IEnumerable<Synapse> synapsesFrom()
        {
            return this.synapses;
        }

		public Number outputValue(Layer prevLayer)
		{
             return   
                 new Sigmoid(
                    new Add(
                        this.synapses.ToList().Select(s => s.value(prevLayer)).ToArray()
                    )
                );
		}

        public bool find(int targetNeurone)
        {
            return targetNeurone == this.index;
        }

        public Error error(IEnumerable<Error> nextErrors, IEnumerable<Synapse> synapses)
        {
            return
                new ExitError(
                    this.index,
                    new Mult(
                        this.value,
                        new Substr(1, this.value),
                        new Add(
                            synapses
                            .Where(s => s.isFromNeurone(this.index))
                            .Select(s => s.error(nextErrors)).ToArray()
                        )
                    )
                );               
        }

        public Neurone withValue(Layer prevLayer)
        {
            return new DeepNeurone(this.index, this.outputValue(prevLayer), this.synapses.ToArray());
        }

        public Neurone withNewSynapse(IEnumerable<Error> nextErrors)
        {
            var neuroneError = nextErrors.ToList().Find(e => e.neuroneIndex() == this.index).asNumber();
            return
                new DeepNeurone(
                    this.index,
                    this.synapses.Select(s => s.withAdjustedWeight(neuroneError, this.value)).ToArray()
                );
                    
        }
	}

 
    public class NullNumber : Number
    {
        public double value()
        {
            throw new Exception("THIS NEURONE HAS NO VALUE");
        }
    }

    public class LinkedLayer
    {
        public LinkedLayer(Layer inputLayer) : this(inputLayer, new List<Layer>(), new List<Layer>().ToArray())
        {
        }


        public LinkedLayer(Layer inputLayer, params Layer[] layers) : this(inputLayer, new List<Layer>(), layers)
        {
        }

        public LinkedLayer(Layer inputLayer, IEnumerable<Layer> outputLayers, params Layer[] layers)
        {
            this.inputLayer = inputLayer;
            this.layers = layers;
            this.outputLayers = outputLayers;
        }

        private Layer inputLayer;
        private IEnumerable<Layer> layers;
        private IEnumerable<Layer> outputLayers;
       
        public Layer lastLayer()
        {
            return this.inputLayer;
        }

        public LinkedLayer linkLayers()
        {       
            if (this.layers.Any())
            {
                return new LinkedLayer(
                    this.layers.First().withPrevLayer(this.inputLayer), 
                    this.layers.Skip(1).ToArray()
                ).linkLayers();
            }
            else
            {
                return new LinkedLayer(this.inputLayer);
            }            
        }
    }

    public class NullLayer : Layer 
    {
        public Number outputValue(int targetNeurone) {
            throw new Exception("YOU HAVE TO LINK YOUR LAYERS");
        }

        public Layer withPrevLayer(Layer layer)
        {
            throw new Exception("CANNOT LINK NULL LAYER");
        }

        public Layer withNextLayer(Layer layer)
        {
            throw new Exception("CANNOT LINK NULL LAYER");
        }

        public IEnumerable<Error> errors(IEnumerable<Error> nextLayerErrors, IEnumerable<Synapse> synapses)
        {
            throw new Exception("CANNOT LINK NULL LAYER");
        }

        public Layer backProp(IEnumerable<Error> errors)
        {
            throw new Exception("CANNOT LINK NULL LAYER");
        }

        public Layer propagate()
        {
            throw new Exception("INPUT NEURONE CANNOT BE AN ERROR");
        }
    }

    public interface Error
    {
        Number asNumber();
        int neuroneIndex();
    }

    public class ExitError : Error
    {
        public ExitError(int index, Number number)
        {
            this.index = index;
            this.number = number;
        }

        private Number number;
        private int index;

        public Number asNumber()
        {
            return this.number;
        }

        public int neuroneIndex()
        {
            return this.index;
        }
    }

    public class OutputExpected : Error
    {
        public OutputExpected(int index, double number) : this(index, new DefaultNumber(number))
        {
        }

        public OutputExpected(int index, Number number)
        {
            this.index = index;
            this.number = number;
        }

        private Number number;
        private int index;

        public Number asNumber()
        {
            return this.number;
        }

        public int neuroneIndex()
        {
            return this.index;
        }
    }

	public class DeepLayer : Layer
	{
        public DeepLayer(params Neurone[] neurones) : this(new NullLayer(), neurones)
		{
		}

        public DeepLayer(Layer prevLayer, params Neurone[] neurones)
        {
            this.prevLayer = prevLayer;
            this.neurones = neurones;
        }

		private IEnumerable<Neurone> neurones;
		private Layer prevLayer;

		public Number outputValue(int originNeurone)
		{
            return neurones.ToList().Find(n => n.find(originNeurone)).outputValue(this.prevLayer);				
		}

        public Layer withPrevLayer(Layer layer)
        {
            return new DeepLayer(layer, this.neurones.ToArray());
        }

        public IEnumerable<Error> errors(IEnumerable<Error> nextLayerErrors, IEnumerable<Synapse> synapses)
        {
            return this.neurones.Select(n => n.error(nextLayerErrors, synapses));
        }

        public Layer propagate()
        {
            return 
                new DeepLayer(
                    prevLayer.propagate(), 
                    this.neurones.Select(n => n.withValue(this.prevLayer)).ToArray()
                );
        }

        public Layer backProp(IEnumerable<Error> errors)
        {
            var newErrors = this.prevLayer.errors(errors, this.neurones.SelectMany(n => n.synapsesFrom()));
            return 
                new DeepLayer(
                    prevLayer.backProp(newErrors), 
                    this.neurones.Select(n => n.withNewSynapse(errors)).ToArray()
                );
        }

	}

	public class InputLayer : Layer
	{
        public InputLayer(params Neurone[] neurones)
		{
            this.neurones = neurones;
		}


        private IEnumerable<Neurone> neurones;

		public Number outputValue(int originNeurone)
		{
            return neurones.ToList().Find(n => n.find(originNeurone)).outputValue(new NullLayer());
		}

        public Layer withPrevLayer(Layer layer)
        {
            throw new Exception("CANNOT LINKED LAYER ON INPUT LAYER");
        }

        public IEnumerable<Error> errors(IEnumerable<Error> nextLayerErrors, IEnumerable<Synapse> synapses)
        {
            return new List<Error>();
        }

        public Layer backProp(IEnumerable<Error> errors)
        {
            return new InputLayer(this.neurones.ToArray());
        }

        public Layer propagate()
        {
            return new InputLayer(this.neurones.ToArray());
        }

	}

	public interface Layer
	{
		Number outputValue(int targetNeurone);
        Layer withPrevLayer(Layer layer);
        Layer propagate();
        Layer backProp(IEnumerable<Error> errors);
        IEnumerable<Error> errors(IEnumerable<Error> nextLayerErrors, IEnumerable<Synapse> synapses);
	}

	public class Synapse
	{
        public Synapse(int originNeurone, int destinNeurone, double weight)
		{
            this.originNeurone = originNeurone;
            this.destinNeurone = destinNeurone;
			this.weight = new DefaultNumber(weight);
		}

        private int originNeurone;
        private int destinNeurone;
		private Number weight;

		public bool isFromNeurone(int neuroneIndex)
		{
            return neuroneIndex == this.originNeurone;
		}

         public Number value(Number input)
        {
            return new Mult(input, this.weight);
        }

		public Number value(Layer prevLayer)
		{
            return this.value(prevLayer.outputValue(this.originNeurone));
		}

        public Number error(IEnumerable<Error> errors)
        {
            return this.value(errors.ToList().Find(e => e.neuroneIndex() == this.destinNeurone).asNumber());
        }

        public Synapse withAdjustedWeight(Number neuroneError, Number neuroneValue)
        {
            return
                new Synapse(
                    this.originNeurone,
                    this.destinNeurone,
                    new Add(
                        this.weight,
                        new Mult(
                            new DefaultNumber(1),
                            neuroneError,
                            neuroneValue
                        )
                    ).value()
                );
        }
	}

	public interface Number
	{
		double value();
	}


	public class DefaultNumber : Number
	{
		public DefaultNumber(double number)
		{
			this.number = number;
		}

		private double number;

		public double value()
		{
			return this.number;
		}
	}

	public class Mult : Number
	{
		public Mult(params Number[] numbers)
		{
			this.numbers = numbers;
		}

		private IEnumerable<Number> numbers;

		public double value()
		{
			return numbers.ToList().Aggregate(1d, (acc, n) => acc * n.value());
		}
	}

    public class Substr : Number
    {
        public Substr(Number number1, double number2) : this(number1, new DefaultNumber(number2))
        {
        }

        public Substr(double number1, Number number2): this(new DefaultNumber(number1), number2 )
        {
        }

        public Substr(Number number1, Number number2)
		{
            this.number1 = number1;
            this.number2 = number2;
		}

        private Number number1;
        private Number number2;

		public double value()
		{
			return this.number1.value() - this.number2.value();
		}
    }

	public class Add : Number
	{
		public Add(params Number[] numbers)
		{
			this.numbers = numbers;
		}

		private Number[] numbers;

		public double value()
		{
			return numbers.ToList().Aggregate(0d, (acc, n) => acc + n.value());
		}
	}

	public class Sigmoid : Number
	{
		public Sigmoid(double toSigmoid) : this(new DefaultNumber(toSigmoid))
		{		
		}

		public Sigmoid(Number toSigmoid)
		{
			this.toSigmoid = toSigmoid;
		}

		private Number toSigmoid;
		public double value()
		{
			return 1 / (1 + Math.Exp(-toSigmoid.value()));
		}
	}


}
