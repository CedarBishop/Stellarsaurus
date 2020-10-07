using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;


namespace Tests
{
    public class GameLogicTests
    {
        [Test]
        public void TestGameManagerAssignNumber()
        {
            GameObject go = new GameObject();
            go.AddComponent<PlayerInputManager>();
            go.AddComponent<FreeForAllGamemode>();
            go.AddComponent<ExtractionGamemode>();
            GameManager gm = go.AddComponent<GameManager>();

            Assert.AreEqual(gm.AssignPlayerNumber(), 1);
            Object.Destroy(gm.gameObject);
        }

        [UnityTest]
        public IEnumerator TestLoader()
        {
            GameObject go = new GameObject();
            Loader loader = go.AddComponent<Loader>();
            yield return new WaitForSeconds(0.1f);
            Assert.NotNull(loader.saveObject);
            Object.Destroy(loader.gameObject);
        }
    }
}
