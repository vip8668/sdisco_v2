(function ($) {
    app.modals.CreateOrEditCashoutMethodTypeModal = function () {

        var _cashoutMethodTypesService = abp.services.app.cashoutMethodTypes;

        var _modalManager;
        var _$cashoutMethodTypeInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$cashoutMethodTypeInformationForm = _modalManager.getModal().find('form[name=CashoutMethodTypeInformationsForm]');
            _$cashoutMethodTypeInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$cashoutMethodTypeInformationForm.valid()) {
                return;
            }

            var cashoutMethodType = _$cashoutMethodTypeInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _cashoutMethodTypesService.createOrEdit(
				cashoutMethodType
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditCashoutMethodTypeModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);