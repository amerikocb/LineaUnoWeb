$(document).ready(function () {
    /*=============SIDEBAR==============*/

    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');

        $('.collapse.show').toggleClass('show');

        $('a[aria-expanded=true]').attr('aria-expanded', 'false');

        $('#sidebar>ul>li>a span#textoopcion').toggleClass('unshow');

        $('#sidebarCollapse').toggleClass('fa-bars');
        $('#sidebarCollapse').toggleClass('fa-times');

        $('header').toggleClass('ml-80px');
        $('header').toggleClass('ml-250px');

        $('main').toggleClass('main-ml-80px');
        $('main').toggleClass('main-ml-250px');

    });

    $('#sidebar').mouseenter(function () {
        if ($('#sidebar').hasClass('active')) {
            $('#sidebar.active>ul>li>a span#textoopcion.unshow').toggleClass('unshow');
            $('header').toggleClass('ml-80px');
            $('header').toggleClass('ml-250px');
        }
    });

    $('#sidebar').mouseleave(function () {
        if ($('#sidebar').hasClass('active')) {
            $('#sidebar.active>ul>li>a span#textoopcion').toggleClass('unshow');

            $('a[aria-expanded=true]').attr('aria-expanded', 'false');

            $('.collapse.show').toggleClass('show');

            $('header').toggleClass('ml-80px');
            $('header').toggleClass('ml-250px');
        }
    });

    /*==================INPUT FILE EXCEL===================*/
    $(".custom-file-input").on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).siblings(".custom-file-label").addClass("selected").html(fileName);

        $("#imagen-excel").html("<img src='/img/logo-excel.png' class='img-fluid' heigth='50px'/>");
        $("#nombre-excel").html("<span>" + fileName + "</span>");
    });
});
