(function($) {     
    $.fn.pager = function(options) {  
        var opts = $.extend({}, $.fn.pager.defaults, options);
        dialogHTML();
        return this.each(function() {         
            $(this).empty().append(renderpager(this.id,parseInt(options.pagenumber), parseInt(options.pagecount), options.buttonClickCallback));                         
            $('.pages li').each(function (i, item) {
                if (i%7 != 2 && i%7 != 5) {
                    $(item).mouseover(function () {
                        //document.body.style.cursor = "pointer";
                        $(this).css("cursor", "pointer");
                    }).mouseout(function () {
                        //document.body.style.cursor = "auto";
                        $(this).css("cursor", "auto");
                    });
                }
            })
        });  
    };

    function dialogHTML() {
        var html = '<div id="paper-error-dialog" style="display:none; ">'
                        + '<p id="paper-error-dialog-text" style="font-family:Microsoft YaHei;font-size:11pt;text-align:center;"></p>'
                  + '</div>';
        $("body").append(html);

        $("#paper-error-dialog").dialog({
            height: 140,
            resizable: false, 
            autoOpen: false,
            width: 280,
            modal: true,
            position: ['center', 250],
            draggabled: true,
            buttons: {
                "确定": function () {
                    $("#paper-error-dialog").dialog("close");
                    $(".paper-input-value").val("")
                }
                    
            }
        });
    }


    function renderpager(id,pagenumber, pagecount, buttonClickCallback) {           
        var $pager = $('<ul class="pages"></ul>');         
        $pager.append(renderButton('首页', pagenumber, pagecount, buttonClickCallback)).append(renderButton('上一页', pagenumber, pagecount, buttonClickCallback));
        // 带数字显示的分页
        /*var startPoint = 1;
        var endPoint = 9;
        var thpoint="<li class='thpoint'>...</li>";
        if (pagenumber > 4) {  
            startPoint = pagenumber - 4;  
            endPoint = pagenumber + 4;  
        }  
        if (endPoint > pagecount) {  
            startPoint = pagecount - 8;  
            endPoint = pagecount;  
            thpoint = "";  
        }  
        if (startPoint < 1) {  
            startPoint = 1;  
        }          
        for (var page = startPoint; page <= endPoint; page++) {  
            var currentButton = $('<li class="page-number">' + (page) + '</li>');  
            page == pagenumber ? currentButton.addClass('pgCurrent') : currentButton.click(function() {  
                buttonClickCallback(this.firstChild.data);  
            });  
            currentButton.appendTo($pager);  
        }*/
        $pager.append("<li class='thpoint' style='cursor:text;'>&nbsp;&nbsp;&nbsp;&nbsp;当前为&nbsp;" + pagenumber +"/" + pagecount + "&nbsp;页&nbsp;&nbsp;&nbsp;&nbsp;</li>");
        $pager.append(renderButton('下一页', pagenumber, pagecount, buttonClickCallback)).append(renderButton('末页', pagenumber, pagecount, buttonClickCallback)); 
        var strgoto = $("<li class='thpoint' style='cursor:text;'>跳转至:&nbsp;<input class='paper-input-value' type='text' value=''maxlength='6' id='gotoval" + "_" + id + "' style='width:30px; height:15px;margin-top:-5px;text-align:center;'/>&nbsp;页</li>");
        $pager.append(strgoto);  
        $pager.append(changepage('跳转', pagecount, buttonClickCallback, id));
        return $pager;  
}      
	function changepage(buttonLabel,pagecount,buttonClickCallback,paperId){  
	    var $btngoto = $('<li class="pgNext" style="font-weight: bolder;color:#868686;">' + buttonLabel + '</li>');
		$btngoto.click(function() {  
		    var gotoval = $('#gotoval' + "_" + paperId).val();
			var patrn = /^[0-9]{1,20}$/;  
			if (!patrn.exec(gotoval)){  
			    //		alert("请输入非零的正整数！");  
			    $("#paper-error-dialog-text").text(LanguageScript.jquery_paper_notZero);
			    $("#paper-error-dialog").dialog("open");
				return false;  
			}  
			var intval = parseInt(gotoval);  
			if(intval > pagecount){  
			  //  alert("您输入的页面超过总页数 " + pagecount);
			    $("#paper-error-dialog-text").text(LanguageScript.jquery_paper_Count + " " + pagecount);
			    $("#paper-error-dialog").dialog("open");
				return ;  
			}
			if (intval == 0)
			{
			//    alert("请输入非零的正整数！   ");
			    $("#paper-error-dialog-text").text(LanguageScript.jquery_paper_notZero);
			    $("#paper-error-dialog").dialog("open");
			    return;
			}
			buttonClickCallback(intval);  
		});  
		return $btngoto;
	}  
	  
	function renderButton(buttonLabel, pagenumber, pagecount, buttonClickCallback) {       
	    var $Button = $('<li class="pgNext" style="font-weight: bolder;color:#868686;">' + buttonLabel + '</li>');
		var destPage = 1;         
		switch (buttonLabel) {
			case "首页":  
			    destPage = 1;
			    $Button.html("|&lt;");
				break;  
			case "上一页":     
			    destPage = pagenumber - 1;
			    $Button.html("&lt;");
				break;  
			case "下一页":  
			    destPage = pagenumber + 1;
			    $Button.html("&gt;");
			break;  
			case "末页":  
			    destPage = pagecount;
			    $Button.html("&gt;|");
			break;       
		}              
		if (buttonLabel == "首页" || buttonLabel == "上一页") {       
			pagenumber <= 1 ? $Button.addClass('pgEmpty') : $Button.click(function() { buttonClickCallback(destPage); });       
		}         
		else {       
			pagenumber >= pagecount ? $Button.addClass('pgEmpty') : $Button.click(function() { buttonClickCallback(destPage); });   
		}
		return $Button;    
	 }      

	 $.fn.pager.defaults = {     
		 pagenumber: 1,       
		 pagecount: 1};  
 })(jQuery);  