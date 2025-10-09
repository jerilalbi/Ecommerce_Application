$(document).ready(function () {
    const contentPlaceholder = $('#contentHolder');
    const navLinks = $('.admin-btn');

    function loadContent(url) {
        $.ajax({
            url: url,
            type: 'GET',
            success: function (response) {
                contentPlaceholder.html(response);
            },
            error: function () {
                contentPlaceholder.html('<div class="alert alert-danger">Error loading content. Please try again.</div>');
            }
        });
    }

    navLinks.on('click', function (e) {
        e.preventDefault();
        const url = $(this).data('url');

        console.log("button clicked");

        navLinks.removeClass('active');
        $(this).addClass('active');

        if (url) {
            loadContent(url);
        }
    });

    $('.sidebar a[data-url="/Admin/Dashboard"]').click();
});