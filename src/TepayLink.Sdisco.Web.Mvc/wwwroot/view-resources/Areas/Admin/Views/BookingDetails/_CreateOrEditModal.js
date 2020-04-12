(function ($) {
    app.modals.CreateOrEditBookingDetailModal = function () {

        var _bookingDetailsService = abp.services.app.bookingDetails;

        var _modalManager;
        var _$bookingDetailInformationForm = null;

		        var _BookingDetailproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BookingDetails/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/BookingDetails/_BookingDetailProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$bookingDetailInformationForm = _modalManager.getModal().find('form[name=BookingDetailInformationsForm]');
            _$bookingDetailInformationForm.validate();
        };

		          $('#OpenProductLookupTableButton').click(function () {

            var bookingDetail = _$bookingDetailInformationForm.serializeFormToObject();

            _BookingDetailproductLookupTableModal.open({ id: bookingDetail.productId, displayName: bookingDetail.productName }, function (data) {
                _$bookingDetailInformationForm.find('input[name=productName]').val(data.displayName); 
                _$bookingDetailInformationForm.find('input[name=productId]').val(data.id); 
            });
        });
		
		$('#ClearProductNameButton').click(function () {
                _$bookingDetailInformationForm.find('input[name=productName]').val(''); 
                _$bookingDetailInformationForm.find('input[name=productId]').val(''); 
        });
		


        this.save = function () {
            if (!_$bookingDetailInformationForm.valid()) {
                return;
            }
            if ($('#BookingDetail_ProductId').prop('required') && $('#BookingDetail_ProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }

            var bookingDetail = _$bookingDetailInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _bookingDetailsService.createOrEdit(
				bookingDetail
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditBookingDetailModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);