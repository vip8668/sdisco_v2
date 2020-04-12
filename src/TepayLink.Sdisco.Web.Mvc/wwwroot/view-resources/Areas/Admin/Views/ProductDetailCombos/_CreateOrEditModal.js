(function ($) {
    app.modals.CreateOrEditProductDetailComboModal = function () {

        var _productDetailCombosService = abp.services.app.productDetailCombos;

        var _modalManager;
        var _$productDetailComboInformationForm = null;

		        var _ProductDetailComboproductLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductDetailCombos/ProductLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ProductDetailCombos/_ProductDetailComboProductLookupTableModal.js',
            modalClass: 'ProductLookupTableModal'
        });        var _ProductDetailComboproductDetailLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/ProductDetailCombos/ProductDetailLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/ProductDetailCombos/_ProductDetailComboProductDetailLookupTableModal.js',
            modalClass: 'ProductDetailLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$productDetailComboInformationForm = _modalManager.getModal().find('form[name=ProductDetailComboInformationsForm]');
            _$productDetailComboInformationForm.validate();
        };

		          $('#OpenProductLookupTableButton').click(function () {

            var productDetailCombo = _$productDetailComboInformationForm.serializeFormToObject();

            _ProductDetailComboproductLookupTableModal.open({ id: productDetailCombo.productId, displayName: productDetailCombo.productName }, function (data) {
                _$productDetailComboInformationForm.find('input[name=productName]').val(data.displayName); 
                _$productDetailComboInformationForm.find('input[name=productId]').val(data.id); 
            });
        });
		
		$('#ClearProductNameButton').click(function () {
                _$productDetailComboInformationForm.find('input[name=productName]').val(''); 
                _$productDetailComboInformationForm.find('input[name=productId]').val(''); 
        });
		
        $('#OpenProductDetailLookupTableButton').click(function () {

            var productDetailCombo = _$productDetailComboInformationForm.serializeFormToObject();

            _ProductDetailComboproductDetailLookupTableModal.open({ id: productDetailCombo.productDetailId, displayName: productDetailCombo.productDetailTitle }, function (data) {
                _$productDetailComboInformationForm.find('input[name=productDetailTitle]').val(data.displayName); 
                _$productDetailComboInformationForm.find('input[name=productDetailId]').val(data.id); 
            });
        });
		
		$('#ClearProductDetailTitleButton').click(function () {
                _$productDetailComboInformationForm.find('input[name=productDetailTitle]').val(''); 
                _$productDetailComboInformationForm.find('input[name=productDetailId]').val(''); 
        });
		
        $('#OpenProduct2LookupTableButton').click(function () {

            var productDetailCombo = _$productDetailComboInformationForm.serializeFormToObject();

            _ProductDetailComboproductLookupTableModal.open({ id: productDetailCombo.itemId, displayName: productDetailCombo.productName2 }, function (data) {
                _$productDetailComboInformationForm.find('input[name=productName2]').val(data.displayName); 
                _$productDetailComboInformationForm.find('input[name=itemId]').val(data.id); 
            });
        });
		
		$('#ClearProductName2Button').click(function () {
                _$productDetailComboInformationForm.find('input[name=productName2]').val(''); 
                _$productDetailComboInformationForm.find('input[name=itemId]').val(''); 
        });
		


        this.save = function () {
            if (!_$productDetailComboInformationForm.valid()) {
                return;
            }
            if ($('#ProductDetailCombo_ProductId').prop('required') && $('#ProductDetailCombo_ProductId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }
            if ($('#ProductDetailCombo_ProductDetailId').prop('required') && $('#ProductDetailCombo_ProductDetailId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('ProductDetail')));
                return;
            }
            if ($('#ProductDetailCombo_ItemId').prop('required') && $('#ProductDetailCombo_ItemId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Product')));
                return;
            }

            var productDetailCombo = _$productDetailComboInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _productDetailCombosService.createOrEdit(
				productDetailCombo
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditProductDetailComboModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);