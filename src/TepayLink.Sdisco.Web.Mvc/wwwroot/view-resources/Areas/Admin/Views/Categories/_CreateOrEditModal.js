(function ($) {
    app.modals.CreateOrEditCategoryModal = function () {

        var _categoriesService = abp.services.app.categories;

        var _modalManager;
        var _$categoryInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$categoryInformationForm = _modalManager.getModal().find('form[name=CategoryInformationsForm]');
            _$categoryInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$categoryInformationForm.valid()) {
                return;
            }

            var category = _$categoryInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _categoriesService.createOrEdit(
				category
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditCategoryModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);