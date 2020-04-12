(function () {
    $(function () {

        var _$chatconversationsTable = $('#ChatconversationsTable');
        var _chatconversationsService = abp.services.app.chatconversations;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Administration.Chatconversations.Create'),
            edit: abp.auth.hasPermission('Pages.Administration.Chatconversations.Edit'),
            'delete': abp.auth.hasPermission('Pages.Administration.Chatconversations.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Chatconversations/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/Admin/Views/Chatconversations/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditChatconversationModal'
        });

		 var _viewChatconversationModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Admin/Chatconversations/ViewchatconversationModal',
            modalClass: 'ViewChatconversationModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$chatconversationsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _chatconversationsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ChatconversationsTableFilter').val(),
					minFriendUserIdFilter: $('#MinFriendUserIdFilterId').val(),
					maxFriendUserIdFilter: $('#MaxFriendUserIdFilterId').val(),
					minUnreadCountFilter: $('#MinUnreadCountFilterId').val(),
					maxUnreadCountFilter: $('#MaxUnreadCountFilterId').val(),
					shardChatConversationIdFilter: $('#ShardChatConversationIdFilterId').val(),
					minBookingIdFilter: $('#MinBookingIdFilterId').val(),
					maxBookingIdFilter: $('#MaxBookingIdFilterId').val(),
					lastMessageFilter: $('#LastMessageFilterId').val(),
					minSideFilter: $('#MinSideFilterId').val(),
					maxSideFilter: $('#MaxSideFilterId').val(),
					minStatusFilter: $('#MinStatusFilterId').val(),
					maxStatusFilter: $('#MaxStatusFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    width: 120,
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
						{
                                text: app.localize('View'),
                                action: function (data) {
                                    _viewChatconversationModal.open({ id: data.record.chatconversation.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.chatconversation.id });
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteChatconversation(data.record.chatconversation);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "chatconversation.userId",
						 name: "userId"   
					},
					{
						targets: 2,
						 data: "chatconversation.friendUserId",
						 name: "friendUserId"   
					},
					{
						targets: 3,
						 data: "chatconversation.unreadCount",
						 name: "unreadCount"   
					},
					{
						targets: 4,
						 data: "chatconversation.shardChatConversationId",
						 name: "shardChatConversationId"   
					},
					{
						targets: 5,
						 data: "chatconversation.bookingId",
						 name: "bookingId"   
					},
					{
						targets: 6,
						 data: "chatconversation.lastMessage",
						 name: "lastMessage"   
					},
					{
						targets: 7,
						 data: "chatconversation.side",
						 name: "side"   
					},
					{
						targets: 8,
						 data: "chatconversation.status",
						 name: "status"   
					}
            ]
        });

        function getChatconversations() {
            dataTable.ajax.reload();
        }

        function deleteChatconversation(chatconversation) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _chatconversationsService.delete({
                            id: chatconversation.id
                        }).done(function () {
                            getChatconversations(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

		$('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewChatconversationButton').click(function () {
            _createOrEditModal.open();
        });

		$('#ExportToExcelButton').click(function () {
            _chatconversationsService
                .getChatconversationsToExcel({
				filter : $('#ChatconversationsTableFilter').val(),
					minFriendUserIdFilter: $('#MinFriendUserIdFilterId').val(),
					maxFriendUserIdFilter: $('#MaxFriendUserIdFilterId').val(),
					minUnreadCountFilter: $('#MinUnreadCountFilterId').val(),
					maxUnreadCountFilter: $('#MaxUnreadCountFilterId').val(),
					shardChatConversationIdFilter: $('#ShardChatConversationIdFilterId').val(),
					minBookingIdFilter: $('#MinBookingIdFilterId').val(),
					maxBookingIdFilter: $('#MaxBookingIdFilterId').val(),
					lastMessageFilter: $('#LastMessageFilterId').val(),
					minSideFilter: $('#MinSideFilterId').val(),
					maxSideFilter: $('#MaxSideFilterId').val(),
					minStatusFilter: $('#MinStatusFilterId').val(),
					maxStatusFilter: $('#MaxStatusFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditChatconversationModalSaved', function () {
            getChatconversations();
        });

		$('#GetChatconversationsButton').click(function (e) {
            e.preventDefault();
            getChatconversations();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getChatconversations();
		  }
		});
    });
})();