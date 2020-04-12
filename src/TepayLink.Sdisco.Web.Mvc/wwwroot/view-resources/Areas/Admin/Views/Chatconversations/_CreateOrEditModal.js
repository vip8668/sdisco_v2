(function ($) {
    app.modals.CreateOrEditChatconversationModal = function () {

        var _chatconversationsService = abp.services.app.chatconversations;

        var _modalManager;
        var _$chatconversationInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$chatconversationInformationForm = _modalManager.getModal().find('form[name=ChatconversationInformationsForm]');
            _$chatconversationInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$chatconversationInformationForm.valid()) {
                return;
            }

            var chatconversation = _$chatconversationInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _chatconversationsService.createOrEdit(
				chatconversation
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditChatconversationModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);