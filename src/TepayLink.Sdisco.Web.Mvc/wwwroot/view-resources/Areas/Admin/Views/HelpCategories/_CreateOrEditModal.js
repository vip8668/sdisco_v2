(function ($) {
    app.modals.CreateOrEditHelpCategoryModal = function () {

        var _helpCategoriesService = abp.services.app.helpCategories;

        var _modalManager;
        var _$helpCategoryInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$helpCategoryInformationForm = _modalManager.getModal().find('form[name=HelpCategoryInformationsForm]');
            _$helpCategoryInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$helpCategoryInformationForm.valid()) {
                return;
            }

            var helpCategory = _$helpCategoryInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _helpCategoriesService.createOrEdit(
				helpCategory
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditHelpCategoryModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);