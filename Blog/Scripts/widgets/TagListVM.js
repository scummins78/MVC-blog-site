var TagListVM = function TagListVM() {

    var self = this;
    self.Tags = ko.observableArray();
    self.IsLoading = ko.observable(false);

    self.Load = function () {
        this.IsLoading(true);

        $.getJSON("/Blog/widget/taglist", function (returnData) {

            if (returnData.Success) {
                self.Tags(returnData.Data);
            }

            self.IsLoading(false);
        });
    };

    // initialize
    this.Load();
};
// TODO:  move this code to a central initialize area.  
$(document).ready(function () {
    var viewModel = new TagListVM();
    ko.applyBindings(viewModel, document.getElementById("tagList"));
});