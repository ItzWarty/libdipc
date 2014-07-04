using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dargon.Ipc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace libdipc.Tests
{
   [TestClass]
   public class LocalNetworkTest
   {
      private LocalNetwork testObj;
      private readonly Guid localNetworkGuid = new Guid(1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

      private Mock<IDipNode> node1 = new Mock<IDipNode>();
      private readonly Guid node1Guid = new Guid(2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
      private Mock<IDipNode> node2 = new Mock<IDipNode>();
      private readonly Guid node2Guid = new Guid(3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
      private Mock<IDipNode> node3 = new Mock<IDipNode>();
      private readonly Guid node3Guid = new Guid(4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
      private Mock<IDipNode> node4 = new Mock<IDipNode>();
      private readonly Guid node4Guid = new Guid(5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

      [TestInitialize]
      public void Setup()
      {
         testObj = new LocalNetwork(localNetworkGuid);
         node1.Setup((n) => n.Guid).Returns(node1Guid);
         node2.Setup((n) => n.Guid).Returns(node2Guid);
         node3.Setup((n) => n.Guid).Returns(node3Guid);
         node4.Setup((n) => n.Guid).Returns(node4Guid);
      }

      [TestMethod]
      public void LocalNetworkRoutesWithinRingNetwork()
      {
         testObj.AddEdge(node1.Object, node2.Object);
         testObj.AddEdge(node2.Object, node3.Object);
         testObj.AddEdge(node3.Object, node4.Object);
         testObj.AddEdge(node4.Object, node1.Object);

         var route = testObj.Route(node1.Object, node3.Object);
      }
   }
}
