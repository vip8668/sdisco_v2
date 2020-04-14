(function ($) {
    app.modals.CreateOrEditRevenueByMonthModal = function () {

        var _revenueByMonthsService = abp.services.app.revenueByMonths;

        var _modalManager;
        var _$revenueByMonthInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$revenueByMonthInformationForm = _modalManager.getModal().find('form[name=RevenueByMonthInformationsForm]');
            _$revenueByMonthInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$revenueByMonthInformationForm.valid()) {
                return;
            }

            var revenueByMonth = _$revenueByMonthInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _revenueByMonthsService.createOrEdit(
				revenueByMonth
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditRevenueByMonthModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);