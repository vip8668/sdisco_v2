(function ($) {
    app.modals.CreateOrEditUtilityModal = function () {

        var _utilitiesService = abp.services.app.utilities;

        var _modalManager;
        var _$utilityInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$utilityInformationForm = _modalManager.getModal().find('form[name=UtilityInformationsForm]');
            _$utilityInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$utilityInformationForm.valid()) {
                return;
            }

            var utility = _$utilityInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _utilitiesService.createOrEdit(
				utility
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditUtilityModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);