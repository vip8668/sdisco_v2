(function ($) {
    app.modals.CreateOrEditBookingRefundModal = function () {

        var _bookingRefundsService = abp.services.app.bookingRefunds;

        var _modalManager;
        var _$bookingRefundInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$bookingRefundInformationForm = _modalManager.getModal().find('form[name=BookingRefundInformationsForm]');
            _$bookingRefundInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$bookingRefundInformationForm.valid()) {
                return;
            }

            var bookingRefund = _$bookingRefundInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _bookingRefundsService.createOrEdit(
				bookingRefund
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditBookingRefundModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);