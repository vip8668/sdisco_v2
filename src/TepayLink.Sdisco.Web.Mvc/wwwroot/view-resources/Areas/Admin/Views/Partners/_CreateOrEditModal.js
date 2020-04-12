(function ($) {
    app.modals.CreateOrEditPartnerModal = function () {

        var _partnersService = abp.services.app.partners;

        var _modalManager;
        var _$partnerInformationForm = null;

		        var _PartneruserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Partners/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Partners/_PartnerUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });        var _PartnerdetinationLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Partners/DetinationLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Partners/_PartnerDetinationLookupTableModal.js',
            modalClass: 'DetinationLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$partnerInformationForm = _modalManager.getModal().find('form[name=PartnerInformationsForm]');
            _$partnerInformationForm.validate();
        };

		          $('#OpenUserLookupTableButton').click(function () {

            var partner = _$partnerInformationForm.serializeFormToObject();

            _PartneruserLookupTableModal.open({ id: partner.userId, displayName: partner.userName }, function (data) {
                _$partnerInformationForm.find('input[name=userName]').val(data.displayName); 
                _$partnerInformationForm.find('input[name=userId]').val(data.id); 
            });
        });
		
		$('#ClearUserNameButton').click(function () {
                _$partnerInformationForm.find('input[name=userName]').val(''); 
                _$partnerInformationForm.find('input[name=userId]').val(''); 
        });
		
        $('#OpenDetinationLookupTableButton').click(function () {

            var partner = _$partnerInformationForm.serializeFormToObject();

            _PartnerdetinationLookupTableModal.open({ id: partner.detinationId, displayName: partner.detinationName }, function (data) {
                _$partnerInformationForm.find('input[name=detinationName]').val(data.displayName); 
                _$partnerInformationForm.find('input[name=detinationId]').val(data.id); 
            });
        });
		
		$('#ClearDetinationNameButton').click(function () {
                _$partnerInformationForm.find('input[name=detinationName]').val(''); 
                _$partnerInformationForm.find('input[name=detinationId]').val(''); 
        });
		


        this.save = function () {
            if (!_$partnerInformationForm.valid()) {
                return;
            }
            if ($('#Partner_UserId').prop('required') && $('#Partner_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            if ($('#Partner_DetinationId').prop('required') && $('#Partner_DetinationId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Detination')));
                return;
            }

            var partner = _$partnerInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _partnersService.createOrEdit(
				partner
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditPartnerModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);