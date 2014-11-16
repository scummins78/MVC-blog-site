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
        }

    };

    return dataService;



});