$(() => {
    const fileUploadArea = $('.avatar-upload-label');
    const previewImageElement = $('.avatar-img-preview');

    const showImagePreview = (event) => {
        const file = event.target.files && event.target.files[0];

        if (file) {
            const reader = new FileReader();
            reader.onload = (readEvent) => {
                fileUploadArea.addClass('hidden');
                previewImageElement
                    .attr('src', readEvent.target.result)
                    .removeClass('hidden');
            };
            reader.readAsDataURL(file);
        }   
    };

    const discardImage = () => {
        $('input[type="file"]').val('');
        fileUploadArea.removeClass('hidden');
        previewImageElement
            .attr('src', '#')
            .addClass('hidden');
    };

    $('input[type="file"]').change(showImagePreview);
    $('#remove-img').click(discardImage);
});