(function ($) {
    app.modals.CreateOrEditShortLinkModal = function () {

        var _shortLinksService = abp.services.app.shortLinks;

        var _modalManager;
        var _$shortLinkInformationForm = null;

		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$shortLinkInformationForm = _modalManager.getModal().find('form[name=ShortLinkInformationsForm]');
            _$shortLinkInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$shortLinkInformationForm.valid()) {
                return;
            }

            var shortLink = _$shortLinkInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _shortLinksService.createOrEdit(
				shortLink
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditShortLinkModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);