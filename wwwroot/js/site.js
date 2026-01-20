$(document).ready(function () {
    // Volume Bar Interaction Logic
    $('.volume-bar-container.interactive').each(function () {
        const $container = $(this);
        const $bars = $container.find('.bar');
        const $feedback = $('#volume-feedback');
        let currentRating = parseInt($container.data('current-volume')) || 0;

        // Hover Effect
        $bars.on('mouseenter', function () {
            const hoverIndex = $(this).data('index');
            updateBars($container, hoverIndex);
            if ($feedback.length) $feedback.text("Level " + hoverIndex);
        });

        $container.on('mouseleave', function () {
            updateBars($container, currentRating);
            if ($feedback.length) $feedback.text(currentRating > 0 ? "Level " + currentRating : "");
        });

        // Click to Set Volume
        $bars.on('click', function () {
            const newRating = $(this).data('index');
            currentRating = newRating;
            $container.data('current-volume', currentRating);
            updateBars($container, currentRating);

            // Pulse effect
            $container.addClass('pulse');
            setTimeout(() => $container.removeClass('pulse'), 300);

            // AJAX Post to save rating
            const albumId = $('#current-album-id').val();
            if (albumId) {
                $.post('/Album/SaveVolume', { albumId: albumId, volumeLevel: currentRating }, function (data) {
                    if ($feedback.length) $feedback.text("Volume Set: " + currentRating);
                }).fail(function () {
                    if ($feedback.length) $feedback.text("Error saving volume");
                });
            }
            console.log("Volume set to:", currentRating);
        });
    });

    function updateBars($container, level) {
        $container.find('.bar').each(function () {
            const index = $(this).data('index');
            if (index <= level) {
                $(this).addClass('active');
            } else {
                $(this).removeClass('active');
            }
        });
    }
});
