(function ($) {
    app.modals.CreateOrEditPartnerRevenueModal = function () {

        var _partnerRevenuesService = abp.services.app.partnerRevenues;

        var _modalManager;
        var _$partnerRevenueInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$partnerRevenueInformationForm = _modalManager.getModal().find('form[name=PartnerRevenueInformationsForm]');
            _$partnerRevenueInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$partnerRevenueInformationForm.valid()) {
                return;
            }

            var partnerRevenue = _$partnerRevenueInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _partnerRevenuesService.createOrEdit(
				partnerRevenue
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditPartnerRevenueModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);