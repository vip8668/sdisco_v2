(function ($) {
    app.modals.CreateOrEditProductModal = function () {

        var _productsService = abp.services.app.products;

        var _modalManager;
        var _$productInformationForm = null;

		        var _ProductcategoryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Products/CategoryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Products/_ProductCategoryLookupTableModal.js',
            modalClass: 'CategoryLookupTableModal'
        });        var _ProductuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Products/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Products/_ProductUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });        var _ProductplaceLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Products/PlaceLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Products/_ProductPlaceLookupTableModal.js',
            modalClass: 'PlaceLookupTableModal'
        });        var _ProductapplicationLanguageLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Products/ApplicationLanguageLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Products/_ProductApplicationLanguageLookupTableModal.js',
            modalClass: 'ApplicationLanguageLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$productInformationForm = _modalManager.getModal().find('form[name=ProductInformationsForm]');
            _$productInformationForm.validate();
        };

		          $('#OpenCategoryLookupTableButton').click(function () {

            var product = _$productInformationForm.serializeFormToObject();

            _ProductcategoryLookupTableModal.open({ id: product.categoryId, displayName: product.categoryName }, function (data) {
                _$productInformationForm.find('input[name=categoryName]').val(data.displayName); 
                _$productInformationForm.find('input[name=categoryId]').val(data.id); 
            });
        });
		
		$('#ClearCategoryNameButton').click(function () {
                _$productInformationForm.find('input[name=categoryName]').val(''); 
                _$productInformationForm.find('input[name=categoryId]').val(''); 
        });
		
        $('#OpenUserLookupTableButton').click(function () {

            var product = _$productInformationForm.serializeFormToObject();

            _ProductuserLookupTableModal.open({ id: product.hostUserId, displayName: product.userName }, function (data) {
                _$productInformationForm.find('input[name=userName]').val(data.displayName); 
                _$productInformationForm.find('input[name=hostUserId]').val(data.id); 
            });
        });
		
		$('#ClearUserNameButton').click(function () {
                _$productInformationForm.find('input[name=userName]').val(''); 
                _$productInformationForm.find('input[name=hostUserId]').val(''); 
        });
		
        $('#OpenPlaceLookupTableButton').click(function () {

            var product = _$productInformationForm.serializeFormToObject();

            _ProductplaceLookupTableModal.open({ id: product.placeId, displayName: product.placeName }, function (data) {
                _$productInformationForm.find('input[name=placeName]').val(data.displayName); 
                _$productInformationForm.find('input[name=placeId]').val(data.id); 
            });
        });
		
		$('#ClearPlaceNameButton').click(function () {
                _$productInformationForm.find('input[name=placeName]').val(''); 
                _$productInformationForm.find('input[name=placeId]').val(''); 
        });
		
        $('#OpenApplicationLanguageLookupTableButton').click(function () {

            var product = _$productInformationForm.serializeFormToObject();

            _ProductapplicationLanguageLookupTableModal.open({ id: product.languageId, displayName: product.applicationLanguageName }, function (data) {
                _$productInformationForm.find('input[name=applicationLanguageName]').val(data.displayName); 
                _$productInformationForm.find('input[name=languageId]').val(data.id); 
            });
        });
		
		$('#ClearApplicationLanguageNameButton').click(function () {
                _$productInformationForm.find('input[name=applicationLanguageName]').val(''); 
                _$productInformationForm.find('input[name=languageId]').val(''); 
        });
		


        this.save = function () {
            if (!_$productInformationForm.valid()) {
                return;
            }
            if ($('#Product_CategoryId').prop('required') && $('#Product_CategoryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Category')));
                return;
            }
            if ($('#Product_HostUserId').prop('required') && $('#Product_HostUserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            if ($('#Product_PlaceId').prop('required') && $('#Product_PlaceId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Place')));
                return;
            }
            if ($('#Product_LanguageId').prop('required') && $('#Product_LanguageId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('ApplicationLanguage')));
                return;
            }

            var product = _$productInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _productsService.createOrEdit(
				product
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditProductModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);