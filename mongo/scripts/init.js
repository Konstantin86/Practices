(function () {

    var database = db.getSiblingDB('epam');

    database.subscribers.drop();
    database.subscribers.insert(getSubscribers());

    function getSubscribers() {
        return [{ "id": 1, "gender": "Female", "first_name": "Judith", "last_name": "Crawford", "email": "jcrawford0@w3.org", "ip_address": "231.157.194.188", "registered": ["bandcamp.com", "sciencedaily.com", "deviantart.com", "360.cn", "moonfruit.com"], "address": { "country": "China", "city": "Damiku", "street": "East" } },
            { "id": 2, "gender": "Female", "first_name": "Jean", "last_name": "Arnold", "email": "jarnold1@deliciousdays.com", "ip_address": "126.134.132.13", "registered": ["creativecommons.org", "sphinn.com", "yahoo.co.jp", "hugedomains.com", "tiny.cc"], "address": { "country": "Lithuania", "city": "Elektrėnai", "street": "Eggendart" } },
            { "id": 3, "gender": "Female", "first_name": "Dorothy", "last_name": "Tucker", "email": "dtucker2@blog.com", "ip_address": "29.99.101.108", "registered": ["domainmarket.com", "salon.com", "google.pl", "fotki.com", "ameblo.jp"], "address": { "country": "Portugal", "city": "Portela", "street": "Corben" } },
            { "id": 4, "gender": "Female", "first_name": "Christine", "last_name": "Reynolds", "email": "creynolds3@phpbb.com", "ip_address": "53.84.4.45", "registered": ["washingtonpost.com", "loc.gov", "macromedia.com", "topsy.com", "slashdot.org"], "address": { "country": "Canada", "city": "South River", "street": "Marcy" } },
            { "id": 5, "gender": "Male", "first_name": "Carlos", "last_name": "Lynch", "email": "clynch4@ustream.tv", "ip_address": "15.232.114.76", "registered": ["hp.com", "wordpress.com", "canalblog.com", "hhs.gov", "yahoo.com"], "address": { "country": "Russia", "city": "Bisert’", "street": "Reinke" } },
            { "id": 6, "gender": "Male", "first_name": "Mark", "last_name": "Phillips", "email": "mphillips5@mail.ru", "ip_address": "205.108.110.70", "registered": ["indiegogo.com", "ehow.com", "bravesites.com", "vimeo.com", "ning.com"], "address": { "country": "Russia", "city": "Krasnyy Kholm", "street": "Lerdahl" } },
            { "id": 7, "gender": "Female", "first_name": "Susan", "last_name": "Little", "email": "slittle6@cnbc.com", "ip_address": "103.206.243.191", "registered": ["scientificamerican.com", "wisc.edu", "about.me", "wsj.com", "ebay.com"], "address": { "country": "China", "city": "Xilai", "street": "Arkansas" } },
            { "id": 8, "gender": "Female", "first_name": "Linda", "last_name": "Dixon", "email": "ldixon7@opensource.org", "ip_address": "183.236.130.138", "registered": ["prlog.org", "princeton.edu", "scientificamerican.com", "zdnet.com", "dot.gov"], "address": { "country": "Poland", "city": "Czarków", "street": "Monica" } },
            { "id": 9, "gender": "Female", "first_name": "Theresa", "last_name": "Harvey", "email": "tharvey8@bloglovin.com", "ip_address": "215.17.232.71", "registered": ["rambler.ru", "unicef.org", "reddit.com", "omniture.com", "accuweather.com"], "address": { "country": "China", "city": "Qimeng", "street": "Namekagon" } },
            { "id": 10, "gender": "Female", "first_name": "Virginia", "last_name": "Freeman", "email": "vfreeman9@harvard.edu", "ip_address": "13.216.43.131", "registered": ["about.com", "sbwire.com", "artisteer.com", "deviantart.com", "taobao.com"], "address": { "country": "Palestinian Territory", "city": "Al ‘Awjā", "street": "Di Loreto" } }];
    }

})();