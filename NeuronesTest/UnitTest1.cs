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
                new LinkedLayer(
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
				new LinkedLayer(
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

			var errors = new Network(resultLayer, new List<Error>() { new OutputExpected(1, 1) }).errors();

			/*Assert.AreEqual(
				errors.First().asNumber().value(),
				1 - expectedValue
			);*/

	
		}



	[TestMethod]
        public void TestNetworkErrorsWithMultipleFor()
        {
		// https://hmkcode.com/ai/backpropagation-step-by-step/
		//https://github.com/hmkcode/netflow.js
			


			var reseau =
                new LinkedLayer(
                    new InputLayer(
                        new InputNeurone(1, 0.05),
                        new InputNeurone(2, 0.10)
                    ),
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
					)
									
				).linkWithPrevLayers().lastLayer();

			

			reseau = reseau.propagate();

			var nt = new Network(reseau, new List<Error>() { new OutputExpected(1, 0.01), new OutputExpected(2, 0.99) });

			var t = new LinkNextLayer(reseau.layerList().Reverse().ToArray()).linkedLayers().firstLayer();

			reseau = t.backProp(nt.errors());

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
