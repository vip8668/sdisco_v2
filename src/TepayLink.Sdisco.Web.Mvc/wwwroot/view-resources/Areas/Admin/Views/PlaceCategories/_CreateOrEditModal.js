(function ($) {
    app.modals.CreateOrEditPlaceCategoryModal = function () {

        var _placeCategoriesService = abp.services.app.placeCategories;

        var _modalManager;
        var _$placeCategoryInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$placeCategoryInformationForm = _modalManager.getModal().find('form[name=PlaceCategoryInformationsForm]');
            _$placeCategoryInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$placeCategoryInformationForm.valid()) {
                return;
            }

            var placeCategory = _$placeCategoryInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _placeCategoriesService.createOrEdit(
				placeCategory
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditPlaceCategoryModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);