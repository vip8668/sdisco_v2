(function ($) {
    app.modals.CreateOrEditBookingModal = function () {

        var _bookingsService = abp.services.app.bookings;

        var _modalManager;
        var _$bookingInformationForm = null;

		        var _BookingproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Bookings/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Bookings/_BookingProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$bookingInformationForm = _modalManager.getModal().find('form[name=BookingInformationsForm]');
            _$bookingInformationForm.validate();
        };

		          $('#OpenProductLookupTableButton').click(function () {

            var booking = _$bookingInformationForm.serializeFormToObject();

            _BookingproductLookupTableModal.open({ id: booking.productId, displayName: booking.productName }, function (data) {
                _$bookingInformationForm.find('input[name=productName]').val(data.displayName); 
                _$bookingInformationForm.find('input[name=productId]').val(data.id); 
            });
        });
		
		$('#ClearProductNameButton').click(function () {
                _$bookingInformationForm.find('input[name=productName]').val(''); 
                _$bookingInformationForm.find('input[name=productId]').val(''); 
        });
		


        this.save = function () {
            if (!_$bookingInformationForm.valid()) {
                return;
            }
            if ($('#Booking_ProductId').prop('required') && $('#Booking_ProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }

            var booking = _$bookingInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _bookingsService.createOrEdit(
				booking
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditBookingModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);