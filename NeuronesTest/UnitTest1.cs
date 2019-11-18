using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neurones;
using System.Linq;

namespace NeuronesTest
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestSigmoid()
		{
			/*var output = new Sigmoid(0.5 * 0.5).value();
            var output2 = new Sigmoid(1 * 0.5 + 2 * 0.5).value();
            var output3 = new Sigmoid(0.81 * 0.5 + 0.81 * 0.5).value();
            var o = 0.81 * (1 - 0.81) * (0.5 + 0.31 + 0.5 * (-0.69));

            var output4 = new Sigmoid(0.8 * 0.5 + 0.2 * 0.5).value();
            var output5 = new Sigmoid(0.62 * 0.5 + 0.62 * 0.5).value();*/
        }

		[TestMethod]
		public void UnNeurone()
		{
			Assert.AreEqual(
				new DeepLayer(
					new InputLayer(
						new InputNeurone(1,0.5)
					),
					new DeepNeurone(
						1,
						new Synapse(1,1, 0.5)
					)
				).neuroneValue(1).value(),
				new Sigmoid(0.5 * 0.5).value()
			);
		}

        [TestMethod]
        public void UnNeuronePropagate()
        {
            Assert.AreEqual(
                new DeepLayer(
                    new InputLayer(
                        new InputNeurone(1, 0.5)
                    ),
                    new DeepNeurone(
                        1,
                        new Synapse(1, 1, 0.5)
                    )
                ).propagate().neuroneValue(1).value(),
                new Sigmoid(0.5 * 0.5).value()
            );
        }

		[TestMethod]
		public void DeuxNeuronesPropagate()
		{
			var inputs =
				new InputLayer(
						new InputNeurone(1,1),
                        new InputNeurone(2,2)
					);

			var resultLayer =
				new DeepLayer(
					inputs,
					new DeepNeurone(
						1,
						new Synapse(1,1, 0.5),
						new Synapse(2,1, 0.5)
					),
                    new DeepNeurone(
						2,
						new Synapse(1,2, 0.5),
						new Synapse(2,2, 0.5)
					)
				);

			Assert.AreEqual(
				resultLayer.propagate().neuroneValue(1).value(),
				new Sigmoid(1 * 0.5 + 2 * 0.5).value()
			);
			Assert.AreEqual(
                resultLayer.propagate().neuroneValue(2).value(),
				new Sigmoid(1 * 0.5 + 2 * 0.5).value()
			);
		}

		[TestMethod]
		public void TwoDeepLayer()
		{
            var inputs =
                new InputLayer(
                        new InputNeurone(1, 1),
                        new InputNeurone(2, 2)
                    );

			var resultLayer =
				new DeepLayer(
					new DeepLayer(
                         new InputLayer(
                            new InputNeurone(1, 0),
                            new InputNeurone(2, 0)
                        ),
                        new DeepNeurone(
							1,
							new Synapse(1,1, 0.5),
							new Synapse(2,1, 0.6)
						),
                        new DeepNeurone(
							2,
							new Synapse(1,2, 0.7),
							new Synapse(2,2, 0.8)
						)
					),
                    new DeepNeurone(
						1,
						new Synapse(1,1, 0.5),
						new Synapse(2,1, 0.6)
					),
                    new DeepNeurone(
						2,
						new Synapse(1,2, 0.7),
						new Synapse(2,2, 0.8)
					)
				);

			Assert.AreEqual(
                resultLayer.withNewSet(inputs).propagate().neuroneValue(1).value(),
				new Sigmoid(
					new Add(				
						new Mult(
							new Sigmoid(1 * 0.5 + 2 * 0.6),
							new DefaultNumber(0.5)
						),
						new Mult(
							new Sigmoid(1 * 0.7 + 2 * 0.8),
							new DefaultNumber(0.6)
						)
					)
				).value()
			);
		}

        [TestMethod]
        public void LinkedLayers()
        {
            var inputs =
                new InputLayer(
                        new InputNeurone(1, 1),
                        new InputNeurone(2, 2)
                    );

            var resultLayer =
                new LinkedPrevLayer(
                    inputs, 
                    new DeepLayer(
                        new DeepNeurone(
							1,
							new Synapse(1,1, 0.5),
							new Synapse(2,1, 0.6)
						),
                        new DeepNeurone(
							2,
							new Synapse(1,2, 0.7),
							new Synapse(2,2, 0.8)
						)
					),
                    new DeepLayer(
                        new DeepNeurone(
							1,
							new Synapse(1,1, 0.5),
							new Synapse(2,1, 0.6)
						),
                        new DeepNeurone(
							2,
							new Synapse(1,2, 0.7),
							new Synapse(2,2, 0.8)
						)
					)
                ).linkWithPrevLayers().lastLayer();

            resultLayer = resultLayer.propagate();
            Assert.AreEqual(
				resultLayer.propagate().neuroneValue(1).value(),
				new Sigmoid(
					new Add(				
						new Mult(
							new Sigmoid(1 * 0.5 + 2 * 0.6),
							new DefaultNumber(0.5)
						),
						new Mult(
							new Sigmoid(1 * 0.7 + 2 * 0.8),
							new DefaultNumber(0.6)
						)
					)
				).value()
			);	
        }

		[TestMethod]
		public void TestNetworkErrors()
		{
			var inputs =
				new InputLayer(
						new InputNeurone(1, 1),
						new InputNeurone(2, 2)
					);

			var resultLayer =
				new LinkedPrevLayer(
					inputs,
					new DeepLayer(
						new DeepNeurone(
							1,
							new Synapse(1, 1, 0.5),
							new Synapse(2, 1, 0.6)
						),
						new DeepNeurone(
							2,
							new Synapse(1, 2, 0.7),
							new Synapse(2, 2, 0.8)
						)
					),
					new DeepLayer(
						new DeepNeurone(
							1,
							new Synapse(1, 1, 0.5),
							new Synapse(2, 1, 0.6)
						)
					)
				).linkWithPrevLayers().lastLayer();

			resultLayer = resultLayer.propagate();

			var expectedValue = 
				new Sigmoid(
					new Add(
						new Mult(
							new Sigmoid(1 * 0.5 + 2 * 0.6),
							new DefaultNumber(0.5)
						),
						new Mult(
							new Sigmoid(1 * 0.7 + 2 * 0.8),
							new DefaultNumber(0.6)
						)
					)
				).value();

			Assert.AreEqual(
				resultLayer.propagate().neuroneValue(1).value(),
				expectedValue
			);

			/*Assert.AreEqual(
				new Network(resultLayer, new List<Error>()
				 {
					new OutputExpected(1, 1),
				 }).errors().First().asNumber().value(), 
				1 - expectedValue
			);*/

		}

		[TestMethod]
        public void TestNetworkWithBackPropagation()
        {
			/*var inputs =
				



		
				new Sigmoid(
					new Add(
						new Mult(
							new Sigmoid(1 * 0.5 + 2 * 0.6),
							new DefaultNumber(0.5)
						),
						new Mult(
							new Sigmoid(1 * 0.7 + 2 * 0.8),
							new DefaultNumber(0.6)
						)
					)
				).value();*/

	

			var errors = 
				new Network(
					new List<Layer>()
					{
						new DeepLayer(
						new DeepNeurone(
							1,
							new Synapse(1, 1, 0.5),
							new Synapse(2, 1, 0.6)
						),
						new DeepNeurone(
							2,
							new Synapse(1, 2, 0.7),
							new Synapse(2, 2, 0.8)
						)
					),
					new OutputLayer(
						new OutputNeurone(
							1,
							new Synapse(1, 1, 0.5),
							new Synapse(2, 1, 0.6)
						)
					)}
				).train(
					new List<TrainingValue>() 
					{ 
						new TrainingValue(
							new InputLayer(
								new InputNeurone(1, 1),
								new InputNeurone(2, 2)
						), 
						new List<ExitError>() 
						{ 
							new ExitError(1, 1)
						}
					)}
				);

			/*Assert.AreEqual(
				errors.First().asNumber().value(),
				1 - expectedValue
			);*/

	
		}

		[TestMethod]
        public void TestNetwork_With_Example()
        {
		// https://hmkcode.com/ai/backpropagation-step-by-step/
		//https://github.com/hmkcode/netflow.js
			

		var network = new Network(
			new List<Layer>() {
				new DeepLayer(
						new DeepNeurone(
							1,
							0.35,
							new Synapse(1, 1, 0.15),
							new Synapse(2, 1, 0.20)
						),
						new DeepNeurone(
							2,
							0.35,
							new Synapse(1, 2, 0.25),
							new Synapse(2, 2, 0.30)
						)
					),
				   new OutputLayer(
						new OutputNeurone(
							1,
							0.6,
							new Synapse(1, 1, 0.40),
							new Synapse(2, 1, 0.45)
						),
						new OutputNeurone(
							2,
							0.6,
							new Synapse(1, 2, 0.50),
							new Synapse(2, 2, 0.55)
						)
					)});



			var trainedNt = network.train(
				new List<TrainingValue>() { 
					new TrainingValue(
						new InputLayer(
							new InputNeurone(1, 0.05),
							new InputNeurone(2, 0.10)
						), 
						new List<ExitError>() { 
							new ExitError(1, 0.01), 
							new ExitError(2, 0.99) 
						})
				});

			/*Assert.IsTrue(trainedNt.neuroneInLayer(1, 1).synapseFrom(1).IsWeightEqualsTo(0.14978071613276281));
			Assert.IsTrue(trainedNt.neuroneInLayer(1, 1).synapseFrom(2).IsWeightEqualsTo(0.19956143226552567));

			Assert.IsTrue(trainedNt.neuroneInLayer(1, 2).synapseFrom(1).IsWeightEqualsTo(0.24975114363236958));
			Assert.IsTrue(trainedNt.neuroneInLayer(1, 2).synapseFrom(2).IsWeightEqualsTo(0.29950228726473915));

			Assert.IsTrue(trainedNt.neuroneInLayer(2, 1).synapseFrom(1).IsWeightEqualsTo(0.35891647971788465));
			Assert.IsTrue(trainedNt.neuroneInLayer(2, 1).synapseFrom(2).IsWeightEqualsTo(0.4086661860762334));

			Assert.IsTrue(trainedNt.neuroneInLayer(2, 2).synapseFrom(1).IsWeightEqualsTo(0.5113012702387375));
			Assert.IsTrue(trainedNt.neuroneInLayer(2, 2).synapseFrom(2).IsWeightEqualsTo(0.56137012110798912));*/
		}

		[TestMethod]
		public void TestNetwork_With_OtherLayers()
		{
			var network = new Network(
			new List<Layer>() {
				new DeepLayer(
						new DeepNeurone(
							1,
							new Synapse(1, 1, 0.10),
							new Synapse(2, 1, 0.15)
						),
						new DeepNeurone(
							2,
							new Synapse(1, 2, 0.25),
							new Synapse(2, 2, 0.30)
						)
					),
				   new OutputLayer(
						new OutputNeurone(
							1,
							new Synapse(1, 1, 0.35),
							new Synapse(2, 1, 0.45)
						),
						new OutputNeurone(
							2,
							new Synapse(1, 2, 0.50),
							new Synapse(2, 2, 0.55)
						)
					)});

			var trainedValues = new List<TrainingValue>();

			var rand = new Random();

			for(var i=0; i<500; i++)
			{
				var val1 = rand.NextDouble();
				var val2 = rand.NextDouble();

				trainedValues.Add(new TrainingValue(
						new InputLayer(
							new InputNeurone(1, val1),
							new InputNeurone(2, val2)
						),
						new List<ExitError>() {
							new ExitError(1, val1 > val2 ? 1 : 0),
							new ExitError(2, val1 > val2 ? 0 : 1)
						}));
			}

			var trainedNt = network.train(trainedValues);
			var result = trainedNt.generalise(new InputLayer(
							new InputNeurone(1, 0.8),
							new InputNeurone(2, 0.2)
						));

			var result2 = trainedNt.generalise(new InputLayer(
							new InputNeurone(1, 0.2),
							new InputNeurone(2, 0.8)
						));


			var r = result.neuroneValue(1).value();
		}
	}

	

    public class DataSet
    {
        public DataSet(Layer inputsData, List<Error> outputExpectedData)
        {
            this.inputsData = inputsData;
            this.outputExpectedData = outputExpectedData;
        }

        private Layer inputsData;
        private List<Error> outputExpectedData;

        public Layer inputs()
        {
            return this.inputsData;
        }

        public List<Error> outputExpected() {
            return this.outputExpectedData;
        }
    }
}
