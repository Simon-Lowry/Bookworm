$('.rating input').change(function () {
    var $radio = $(this);
    $('.rating .selected').removeClass('selected');
    $radio.closest('label').addClass('selected');
});


$(document).ready(function () {
    var value = $("#Rating").val();
    $('.rating .selected').removeClass('selected');

    if (value === "1") {
        var $radio = $(document.getElementById('rating1'));
        $radio.closest('label').addClass('selected');
    } else if (value === "2") {
        var $radio = $(document.getElementById('rating2'));
        $radio.closest('label').addClass('selected');
    } else if (value === "3") {
        var $radio = $(document.getElementById('rating3'));
        $radio.closest('label').addClass('selected');
    } else if (value === "4") {
        var $radio = $(document.getElementById('rating4'));
        $radio.closest('label').addClass('selected');
    } else if (value === "5") {
        var $radio = $(document.getElementById('rating5'));
        $radio.closest('label').addClass('selected');
    }

    document.getElementById('rating1').value = "5";
});

