(function ($) {
    app.modals.CreateOrEditChatMessageV2Modal = function () {

        var _chatMessageV2sService = abp.services.app.chatMessageV2s;

        var _modalManager;
        var _$chatMessageV2InformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$chatMessageV2InformationForm = _modalManager.getModal().find('form[name=ChatMessageV2InformationsForm]');
            _$chatMessageV2InformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$chatMessageV2InformationForm.valid()) {
                return;
            }

            var chatMessageV2 = _$chatMessageV2InformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _chatMessageV2sService.createOrEdit(
				chatMessageV2
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditChatMessageV2ModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);