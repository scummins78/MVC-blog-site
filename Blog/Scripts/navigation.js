$(window).scroll(function () {
    if ($(".navbar").offset().top > 80) {
        $(".navbar-fixed-top").addClass("top-nav-collapse");
    } else {
        $(".navbar-fixed-top").removeClass("top-nav-collapse");
    }
});

$(function () {
    $('#scrollTop').click(function () {
        var target = $('#blog-items');
        if (target.length) {
            $('html,body').animate({
                scrollTop: target.offset().top
            }, 500);
            return false;
        }
    });
});