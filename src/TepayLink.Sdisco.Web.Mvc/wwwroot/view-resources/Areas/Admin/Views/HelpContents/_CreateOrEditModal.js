(function ($) {
    app.modals.CreateOrEditHelpContentModal = function () {

        var _helpContentsService = abp.services.app.helpContents;

        var _modalManager;
        var _$helpContentInformationForm = null;

		        var _HelpContenthelpCategoryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/HelpContents/HelpCategoryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/HelpContents/_HelpContentHelpCategoryLookupTableModal.js',
            modalClass: 'HelpCategoryLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$helpContentInformationForm = _modalManager.getModal().find('form[name=HelpContentInformationsForm]');
            _$helpContentInformationForm.validate();
        };

		          $('#OpenHelpCategoryLookupTableButton').click(function () {

            var helpContent = _$helpContentInformationForm.serializeFormToObject();

            _HelpContenthelpCategoryLookupTableModal.open({ id: helpContent.helpCategoryId, displayName: helpContent.helpCategoryCategoryName }, function (data) {
                _$helpContentInformationForm.find('input[name=helpCategoryCategoryName]').val(data.displayName); 
                _$helpContentInformationForm.find('input[name=helpCategoryId]').val(data.id); 
            });
        });
		
		$('#ClearHelpCategoryCategoryNameButton').click(function () {
                _$helpContentInformationForm.find('input[name=helpCategoryCategoryName]').val(''); 
                _$helpContentInformationForm.find('input[name=helpCategoryId]').val(''); 
        });
		


        this.save = function () {
            if (!_$helpContentInformationForm.valid()) {
                return;
            }
            if ($('#HelpContent_HelpCategoryId').prop('required') && $('#HelpContent_HelpCategoryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('HelpCategory')));
                return;
            }

            var helpContent = _$helpContentInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _helpContentsService.createOrEdit(
				helpContent
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditHelpContentModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);