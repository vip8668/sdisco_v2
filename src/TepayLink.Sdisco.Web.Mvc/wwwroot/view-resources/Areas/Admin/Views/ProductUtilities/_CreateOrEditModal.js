(function ($) {
    app.modals.CreateOrEditProductUtilityModal = function () {

        var _productUtilitiesService = abp.services.app.productUtilities;

        var _modalManager;
        var _$productUtilityInformationForm = null;

		        var _ProductUtilityproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductUtilities/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ProductUtilities/_ProductUtilityProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });        var _ProductUtilityutilityLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductUtilities/UtilityLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ProductUtilities/_ProductUtilityUtilityLookupTableModal.js',
            modalClass: 'UtilityLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$productUtilityInformationForm = _modalManager.getModal().find('form[name=ProductUtilityInformationsForm]');
            _$productUtilityInformationForm.validate();
        };

		          $('#OpenProductLookupTableButton').click(function () {

            var productUtility = _$productUtilityInformationForm.serializeFormToObject();

            _ProductUtilityproductLookupTableModal.open({ id: productUtility.productId, displayName: productUtility.productName }, function (data) {
                _$productUtilityInformationForm.find('input[name=productName]').val(data.displayName); 
                _$productUtilityInformationForm.find('input[name=productId]').val(data.id); 
            });
        });
		
		$('#ClearProductNameButton').click(function () {
                _$productUtilityInformationForm.find('input[name=productName]').val(''); 
                _$productUtilityInformationForm.find('input[name=productId]').val(''); 
        });
		
        $('#OpenUtilityLookupTableButton').click(function () {

            var productUtility = _$productUtilityInformationForm.serializeFormToObject();

            _ProductUtilityutilityLookupTableModal.open({ id: productUtility.utilityId, displayName: productUtility.utilityName }, function (data) {
                _$productUtilityInformationForm.find('input[name=utilityName]').val(data.displayName); 
                _$productUtilityInformationForm.find('input[name=utilityId]').val(data.id); 
            });
        });
		
		$('#ClearUtilityNameButton').click(function () {
                _$productUtilityInformationForm.find('input[name=utilityName]').val(''); 
                _$productUtilityInformationForm.find('input[name=utilityId]').val(''); 
        });
		


        this.save = function () {
            if (!_$productUtilityInformationForm.valid()) {
                return;
            }
            if ($('#ProductUtility_ProductId').prop('required') && $('#ProductUtility_ProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }
            if ($('#ProductUtility_UtilityId').prop('required') && $('#ProductUtility_UtilityId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Utility')));
                return;
            }

            var productUtility = _$productUtilityInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _productUtilitiesService.createOrEdit(
				productUtility
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditProductUtilityModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);