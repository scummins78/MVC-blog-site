define("DataService", ["config"], function (config) {


    var dataService = {
        getArchiveItems: function () {
            var deferred = $.Deferred();

            $.getJSON(config.baseUrl + "api/archive", function (returnData) {

                if (returnData.Success) {
                    deferred.resolve(returnData.Data);
                }
            });

            return deferred.promise();
        },
        getRecentPosts: function () {
            var deferred = $.Deferred();

            $.getJSON(config.baseUrl + "api/posts/recent", function (returnData) {

                if (returnData.Success) {
                    deferred.resolve(returnData.Data);
                }
            });

            return deferred.promise();
        },
        getTags: function () {
            var deferred = $.Deferred();

            $.getJSON(config.baseUrl + "api/tags", function (returnData) {

                if (returnData.Success) {
                    deferred.resolve(returnData.Data);
                }
            });

            return deferred.promise();
        },
        getBlogs: function(page, itemsPerPage, sort) {
            var deferred = $.Deferred();

            var url = config.baseUrl + "api/posts/page/" + page + "?itemsPerPage=" + itemsPerPage;
            if (sort !== undefined) {
                url = url + "&sort=" + sort;
            }

            $.getJSON(url, function (returnData) {

                if (returnData.Success) {
                    deferred.resolve(returnData.Data);
                }
            });

            return deferred.promise();
        }
    };

    return dataService;



});