(function () {

    var database = db.getSiblingDB('epam');

    database.subscribers.drop();
    database.subscribers.insert(getSubscribers());

    function getSubscribers() {
        return [
          { _id:1, name: "Carl", gender:"male", visits:[{site: "vk", count: 30}, {site: "facebook", count: 20}, {site: "google", count: 200}] },  
          { _id:2, name: "David", gender:"male", visits:[{site: "vk", count: 44}, {site: "facebook", count: 50}] },  
          { _id:3, name: "Adam", gender:"male", visits:[{site: "vk", count: 25}, {site: "facebook", count: 80}] },  
          { _id:4, name: "Luke", gender:"male", visits:[{site: "vk", count: 53}, {site: "facebook", count: 31}] },  
          { _id:5, name: "Asya", gender:"female", visits:[{site: "vk", count: 22}, {site: "facebook", count: 11}, {site: "google", count: 500}] },  
          { _id:6, name: "Marta", gender:"female", visits:[{site: "vk", count: 5}, {site: "facebook", count: 200}] }  
        ];
    }

})();