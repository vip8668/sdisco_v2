(function ($) {
    app.modals.CreateOrEditBookingClaimModal = function () {

        var _bookingClaimsService = abp.services.app.bookingClaims;

        var _modalManager;
        var _$bookingClaimInformationForm = null;

		        var _BookingClaimclaimReasonLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/BookingClaims/ClaimReasonLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/BookingClaims/_BookingClaimClaimReasonLookupTableModal.js',
            modalClass: 'ClaimReasonLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$bookingClaimInformationForm = _modalManager.getModal().find('form[name=BookingClaimInformationsForm]');
            _$bookingClaimInformationForm.validate();
        };

		          $('#OpenClaimReasonLookupTableButton').click(function () {

            var bookingClaim = _$bookingClaimInformationForm.serializeFormToObject();

            _BookingClaimclaimReasonLookupTableModal.open({ id: bookingClaim.claimReasonId, displayName: bookingClaim.claimReasonTitle }, function (data) {
                _$bookingClaimInformationForm.find('input[name=claimReasonTitle]').val(data.displayName); 
                _$bookingClaimInformationForm.find('input[name=claimReasonId]').val(data.id); 
            });
        });
		
		$('#ClearClaimReasonTitleButton').click(function () {
                _$bookingClaimInformationForm.find('input[name=claimReasonTitle]').val(''); 
                _$bookingClaimInformationForm.find('input[name=claimReasonId]').val(''); 
        });
		


        this.save = function () {
            if (!_$bookingClaimInformationForm.valid()) {
                return;
            }
            if ($('#BookingClaim_ClaimReasonId').prop('required') && $('#BookingClaim_ClaimReasonId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('ClaimReason')));
                return;
            }

            var bookingClaim = _$bookingClaimInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _bookingClaimsService.createOrEdit(
				bookingClaim
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditBookingClaimModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);