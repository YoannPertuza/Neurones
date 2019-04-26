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
		public void DeuxNeurones()
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
						new Synapse(2,1, 0.6)
					),
                    new DeepNeurone(
						2,
						new Synapse(1,2, 0.7),
						new Synapse(2,2, 0.8)
					)
				);

			Assert.AreEqual(
				resultLayer.outputValue(1).value(),
				new Sigmoid(1 * 0.5 + 2 * 0.6).value()
			);
			Assert.AreEqual(
				resultLayer.outputValue(2).value(),
				new Sigmoid(1 * 0.7 + 2 * 0.8).value()
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
						inputs,
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
				resultLayer.outputValue(1).value(),
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

            Assert.AreEqual(
				resultLayer.outputValue(1).value(),
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
                        new InputNeurone(1, 10),
                        new InputNeurone(2, 10)
                    );

            var lastLayer =  
                new LinkedLayer(
                        inputs,
                        new DeepLayer(
                            new DeepNeurone(
                                1,
                                new Synapse(1,1, 1),
                                new Synapse(2,1, 1)
                            ),
                            new DeepNeurone(
                                2,
                                new Synapse(1,2, 1),
                                new Synapse(2,2, 1)
                            )
                        ),
                        new DeepLayer(
                            new DeepNeurone(
                                1,
                                new Synapse(1,1, 1),
                                new Synapse(2,1, 1)
                            ),
                            new DeepNeurone(
                                2,
                                new Synapse(1,2, 1),
                                new Synapse(2,2, 1)
                            )
                        )
                    ).linkLayers().lastLayer().propagate();

            var error =
                new Network(
                    lastLayer,
                        new List<Error>()
                            {
                                new OutputExpected(1, 1),
                                new OutputExpected(2, 2),
                            }
                        ).errors();

            lastLayer = lastLayer.backProp(error);

       
         for (var i = 0; i < 10; i++)
         {
             lastLayer = lastLayer.propagate();

             error = new Network(
                    lastLayer,
                        new List<Error>()
                            {
                                new OutputExpected(1, 1),
                                new OutputExpected(2, 2),
                            }
                        ).errors();

            lastLayer = lastLayer.backProp(error);

         }


         var value = lastLayer.outputValue(1).value();
        }
	}
}
