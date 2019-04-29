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
			var output = new Sigmoid(0.5 * 0.5).value();
            var output2 = new Sigmoid(1 * 0.5 + 2 * 0.5).value();
            var output3 = new Sigmoid(0.81 * 0.5 + 0.81 * 0.5).value();
            var o = 0.81 * (1 - 0.81) * (0.5 + 0.31 + 0.5 * (-0.69));

            var output4 = new Sigmoid(0.8 * 0.5 + 0.2 * 0.5).value();
            var output5 = new Sigmoid(0.62 * 0.5 + 0.62 * 0.5).value();
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
				).outputValue(1).value(),
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
                ).propagate().outputValue(1).value(),
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
				resultLayer.propagate().outputValue(1).value(),
				new Sigmoid(1 * 0.5 + 2 * 0.5).value()
			);
			Assert.AreEqual(
                resultLayer.propagate().outputValue(2).value(),
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
                resultLayer.withNewSet(inputs).propagate().outputValue(1).value(),
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
                new LinkedLayer(
                    inputs, 
                    new DeepLayer(
                        new DeepNeurone(
							1,
							new Synapse(1,1, 0.5),
							new Synapse(2,2, 0.6)
						),
                        new DeepNeurone(
							2,
							new Synapse(1,1, 0.7),
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
                ).linkLayers().lastLayer();

            resultLayer = resultLayer.propagate();
            Assert.AreEqual(
				resultLayer.propagate().outputValue(1).value(),
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
        public void TestNetworkErrorsTbk()
        {
           var prog =
             new LinkedLayer(
                  new InputLayer(
                      new InputNeurone(1, 0.8),
                      new InputNeurone(2, 0.2)
                  ),
                  new DeepLayer(
                      new DeepNeurone(
                          1,
                          new Synapse(1, 1, 0.5),
                          new Synapse(2, 1, 0.6)
                      ),
                      new DeepNeurone(
                          2,
                          new Synapse(1, 2, 0.4),
                          new Synapse(2, 2, 0.7)
                      )
                  ),
                  new OutputLayer(
                      new DeepNeurone(
                          1,
                          new Synapse(1, 1, 0.7),
                          new Synapse(2, 1, 0.4)
                      )
                  )
              ).linkLayers().lastLayer().propagate();

           var errors = 
               new Network(prog, new List<Error>()
                {
                    new OutputExpected(1, 1),
                }).errors();

           var back = prog.backProp(errors, new List<Synapse>());
        
        }
             

        [TestMethod]
        public void TestNetworkErrors()
        {
            var dataSets = new List<DataSet>();

            for (var i = 1; i < 8000; i++)
            {
                var r = new Random();
                var a = r.NextDouble();
                var b = r.NextDouble();
                dataSets.Add(
                    new DataSet(
                        new InputLayer(
                            new InputNeurone(1, a),
                            new InputNeurone(2, b)
                        ),
                        new List<Error>()
                        {
                            new OutputExpected(1, a > b ? 1 : 0)
                        }
                    )
                );
            }

            var rs = new Random();

            var reseau =
                new LinkedLayer(
                    new InputLayer(
                        new InputNeurone(1, 0),
                        new InputNeurone(2, 0)
                    ),
                    new DeepLayer(
                        new DeepNeurone(
                            1,
                            new Synapse(1, 1, rs.NextDouble()),
                            new Synapse(2, 1, rs.NextDouble())
                        ),
                        new DeepNeurone(
                            2,
                            new Synapse(1, 2, rs.NextDouble()),
                            new Synapse(2, 2, rs.NextDouble())
                        )
                    ),
                    new DeepLayer(
                        new DeepNeurone(
                            1,
                            new Synapse(1, 1, rs.NextDouble()),
                            new Synapse(2, 1, rs.NextDouble())
                        ),
                        new DeepNeurone(
                            2,
                            new Synapse(1, 2, rs.NextDouble()),
                            new Synapse(2, 2, rs.NextDouble())
                        ),
                        new DeepNeurone(
                            3,
                            new Synapse(1, 3, rs.NextDouble()),
                            new Synapse(2, 3, rs.NextDouble())
                        ),
                        new DeepNeurone(
                            4,
                            new Synapse(1, 4, rs.NextDouble()),
                            new Synapse(2, 4, rs.NextDouble())
                        )
                    ),
                    new OutputLayer(
                        new DeepNeurone(
                            1,
                            new Synapse(1, 1, rs.NextDouble()),
                            new Synapse(2, 1, rs.NextDouble()),
                            new Synapse(3, 1, rs.NextDouble()),
                            new Synapse(4, 1, rs.NextDouble())
                        )
                    )
                ).linkLayers().lastLayer();

            IEnumerable<Error> errors = null;

            foreach (var dataSet in dataSets)
            {
                reseau = reseau.withNewSet(dataSet.inputs()).propagate();
                errors = new Network(reseau, dataSet.outputExpected()).errors();                
                reseau = reseau.backProp(errors, new List<Synapse>());
            }

            var check1 = reseau.withNewSet(
                new InputLayer(
                        new InputNeurone(1, 1),
                        new InputNeurone(2, 0)
                    )).propagate().outputValue(1).value();

            var check2 = reseau.withNewSet(
               new InputLayer(
                       new InputNeurone(1, 0.1),
                       new InputNeurone(2, 0.8)
                   )).propagate().outputValue(1).value();

            var check3 = reseau.withNewSet(
               new InputLayer(
                       new InputNeurone(1, 0.5),
                       new InputNeurone(2, 0.6)
                   )).propagate().outputValue(1).value();
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
