define("DataService", [], function () {


    var dataService = {
        getArchiveItems: function () {
            var deferred = $.Deferred();

            $.getJSON("/Blog/widget/archivelist", function (returnData) {

                if (returnData.Success) {
                    deferred.resolve(returnData.Data);
                }
            });

            return deferred.promise();
        },
        getRecentPosts: function () {
            var deferred = $.Deferred();

            $.getJSON("/Blog/widget/recentlist", function (returnData) {

                if (returnData.Success) {
                    deferred.resolve(returnData.Data);
                }
            });

            return deferred.promise();
        },
        getTags: function () {
            var deferred = $.Deferred();

            $.getJSON("/Blog/widget/taglist", function (returnData) {

                if (returnData.Success) {
                    deferred.resolve(returnData.Data);
                }
            });

            return deferred.promise();
        },
        getBlogs: function(page, itemsPerPage, sort) {
            var deferred = $.Deferred();

            var url = "/Blog/dashboard/bloglist/" + page + "?itemsPerPage=" + itemsPerPage;
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