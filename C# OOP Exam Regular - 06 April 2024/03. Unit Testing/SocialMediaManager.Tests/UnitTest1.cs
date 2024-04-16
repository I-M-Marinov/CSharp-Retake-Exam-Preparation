using System;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;

namespace SocialMediaManager.Tests
{
    public class Tests
    {

       

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void RegisterInfluencer_ValidInfluencer_SuccessfullyAdded()
        {
            var repository = new InfluencerRepository();
            var influencer = new Influencer("testuser", 1000);

            string result = repository.RegisterInfluencer(influencer);

            Assert.AreEqual("Successfully added influencer testuser with 1000", result);
            Assert.AreEqual(1, repository.Influencers.Count);
            Assert.AreEqual(influencer, repository.GetInfluencer("testuser"));
        }

        [Test]
        public void RegisterInfluencer_NullInfluencer_ThrowsArgumentNullException()
        {
            var repository = new InfluencerRepository();

            Assert.Throws<ArgumentNullException>(() => repository.RegisterInfluencer(null));
        }

        [Test]
        public void RegisterInfluencer_DuplicateUsername_ThrowsInvalidOperationException()
        {
            var repository = new InfluencerRepository();
            var influencer1 = new Influencer("testuser", 1000);
            var influencer2 = new Influencer("testuser", 2000);

            repository.RegisterInfluencer(influencer1);

      
            Assert.Throws<InvalidOperationException>(() => repository.RegisterInfluencer(influencer2));
        }

        [Test]
        public void RemoveInfluencer_ValidUsername_RemovesInfluencer()
        {
            var repository = new InfluencerRepository();
            var influencer = new Influencer("testuser", 1000);
            repository.RegisterInfluencer(influencer);


            bool isRemoved = repository.RemoveInfluencer("testuser");

            Assert.IsTrue(isRemoved);
            Assert.AreEqual(0, repository.Influencers.Count);
        }

        [Test]
        public void RemoveInfluencer_NullUsername_ThrowsArgumentNullException()
        {
            var repository = new InfluencerRepository();
            Assert.Throws<ArgumentNullException>(() => repository.RemoveInfluencer(null));
        }

        [Test]
        public void GetInfluencerWithMostFollowers_ReturnsInfluencerWithMostFollowers()
        {
            var repository = new InfluencerRepository();
            var influencer1 = new Influencer("user1", 2000);
            var influencer2 = new Influencer("user2", 3000);
            repository.RegisterInfluencer(influencer1);
            repository.RegisterInfluencer(influencer2);

            var result = repository.GetInfluencerWithMostFollowers();

            Assert.AreEqual("user2", result.Username);
            Assert.AreEqual(3000, result.Followers);
        }

        [Test]
        public void GetInfluencer_ReturnsInfluencerWithGivenUsername()
        {
            var repository = new InfluencerRepository();
            var influencer = new Influencer("testuser", 1000);
            repository.RegisterInfluencer(influencer);


            var result = repository.GetInfluencer("testuser");


            Assert.AreEqual("testuser", result.Username);
            Assert.AreEqual(1000, result.Followers);
        }

        [Test]
        public void GetInfluencer_NonExistentUsername_ReturnsNull()
        {

            var repository = new InfluencerRepository();
            var result = repository.GetInfluencer("nonexistent");

            Assert.AreEqual(null,result);
        }
    }
}