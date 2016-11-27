//site.js
(function () {
    //var ele = $("#username");
    //ele.text("Rahmouni Imad");


    //var main = $("#main");
    //main.on("mouseenter",function() {
    //    main.style = "background-color: #888";
    //});
    //main.on("mouseleave", function () {
    //    main.style = "";
    //});


    //var menuItems = $("ul.menu li a");
    //menuItems.on("click", function () {
    //    var me = $(this);
    //    alert(me.text());
    //});

    var $sidebarAndWrapper = $("#sidebar,#wrapper");
    var $icon = $("#sidebarToggle i.glyphicon");
    $("#sidebarToggle").on("click", function() {
        $sidebarAndWrapper.toggleClass("hide-sidebar");
        if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
            $icon.removeClass("glyphicon-chevron-left");
            $icon.addClass("glyphicon-chevron-right");
        } else {
            $icon.addClass("glyphicon-chevron-left");
            $icon.removeClass("glyphicon-chevron-right");
        }
    });

})();