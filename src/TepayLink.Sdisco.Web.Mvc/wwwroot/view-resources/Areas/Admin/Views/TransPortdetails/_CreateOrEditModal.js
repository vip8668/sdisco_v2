(function ($) {
    app.modals.CreateOrEditTransPortdetailModal = function () {

        var _transPortdetailsService = abp.services.app.transPortdetails;

        var _modalManager;
        var _$transPortdetailInformationForm = null;

		        var _TransPortdetailproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/TransPortdetails/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/TransPortdetails/_TransPortdetailProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$transPortdetailInformationForm = _modalManager.getModal().find('form[name=TransPortdetailInformationsForm]');
            _$transPortdetailInformationForm.validate();
        };

		          $('#OpenProductLookupTableButton').click(function () {

            var transPortdetail = _$transPortdetailInformationForm.serializeFormToObject();

            _TransPortdetailproductLookupTableModal.open({ id: transPortdetail.productId, displayName: transPortdetail.productName }, function (data) {
                _$transPortdetailInformationForm.find('input[name=productName]').val(data.displayName); 
                _$transPortdetailInformationForm.find('input[name=productId]').val(data.id); 
            });
        });
		
		$('#ClearProductNameButton').click(function () {
                _$transPortdetailInformationForm.find('input[name=productName]').val(''); 
                _$transPortdetailInformationForm.find('input[name=productId]').val(''); 
        });
		


        this.save = function () {
            if (!_$transPortdetailInformationForm.valid()) {
                return;
            }
            if ($('#TransPortdetail_ProductId').prop('required') && $('#TransPortdetail_ProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }

            var transPortdetail = _$transPortdetailInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _transPortdetailsService.createOrEdit(
				transPortdetail
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditTransPortdetailModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);