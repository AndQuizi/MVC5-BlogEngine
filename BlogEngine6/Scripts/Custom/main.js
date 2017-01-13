function deleteComment_Confirm(e){var t="<h3> Are you sure you want to feature this comment?</h3>";bootbox.confirm(t,function(t){t&&deleteComment_Delete(e)})}function deleteComment_Delete(e){var t=$(e).parents("form");$.ajax({type:"POST",url:"/Editor/RemoveComment",data:t.serialize(),dataType:"json",success:function(e){e.success&&$(".commentItem").filter('[data-id="'+e.commentId+'"]').remove()}})}function featureBlogItem_Confirm(e){var t=$(e).parents("form").find('input[name="id"]').val();$.ajax({type:"GET",url:"/Blogs/DetailsJSON",data:{id:t},dataType:"json",success:function(t){var o="<h3> Are you sure you want to feature this blog post?</h3><h4>Title:</h4><p>"+t.Title+"</p><br/><h4>Post Date:</h4><p>"+t.PostDate+"</p>";bootbox.confirm(o,function(t){t&&featureBlogItem_Feature(e)})}})}function featureBlogItem_Feature(e){var t=$(e).parents("form");$.ajax({type:"POST",url:"/Editor/AddFeaturedArticle",data:t.serialize(),dataType:"json",success:function(e){bootbox.alert("<h3>"+(e.success?"Success":"Error")+"</h3><p>"+e.msg+"</p>")}})}function removeFeaturedBlog_Confirm(e){var t=$(e).parents("form").find('input[name="id"]').val();$.ajax({type:"GET",url:"/Blogs/DetailsJSON",data:{id:t},dataType:"json",success:function(t){var o="<h3> Are you sure you want to remove this featured blog post?</h3><h4>Title:</h4><p>"+t.Title+"</p><br/><h4>Post Date:</h4><p>"+t.PostDate+"</p>";bootbox.confirm(o,function(t){t&&removeFeaturedBlog_Remove(e)})}})}function removeFeaturedBlog_Remove(e){var t=$(e).parents("form");$.ajax({type:"POST",url:"/Editor/RemoveFeaturedArticle",data:t.serialize(),dataType:"json",success:function(e){e.success&&$(".blogItem").filter('[data-id="'+e.blogId+'"]').remove()}})}function deleteBlogItem_Confirm(e,t){var o=$(e).parents("form").find('input[name="id"]').val();$.ajax({type:"GET",url:"/Blogs/DetailsJSON",data:{id:o},dataType:"json",success:function(o){var a="<h3> Are you sure you want to delete this blog post?</h3><h4>Title:</h4><p>"+o.Title+"</p><br/><h4>Post Date:</h4><p>"+o.PostDate+"</p>";bootbox.confirm(a,function(o){o&&deleteBlogItem_Delete(e,t)})}})}function deleteBlogItem_Delete(e,t){var o=$(e).parents("form");$.ajax({type:"POST",url:"/MyBlog/Delete",data:o.serialize(),dataType:"json",success:function(e){t?window.location.replace("/MyBlog/Index"):$(".blogItem").filter('[data-id="'+e.blogId+'"]').remove()}})}function favoriteBlogItem(e){var t=$(e).parents("form");$.ajax({type:"POST",url:"/Blogs/FavoritePost",data:t.serialize(),dataType:"json",success:function(t){t.success?t.isAdded?$(e).addClass("btn-danger"):$(e).removeClass("btn-danger"):bootbox.alert("<p><b>"+t.msg+"</b></p>")}})}function postBlogComment(e,t){var o=$(e).parents("form");$.ajax({type:"POST",url:"/Blogs/PostComment",data:o.serialize(),dataType:"json",success:function(e){e.success?(o.find("textarea").val(""),$(".blogCommentArea").load("/Blogs/BlogComments?id="+e.blogId+"&count="+t)):bootbox.alert("<p><b>"+e.msg+"</b></p>")}})}$(document).ready(function(){"undefined"==typeof IS_AUTH&&$(".navbar-nav li a.requireLogin").click(function(e){return e.preventDefault(),$("#ModalLogin").modal("show"),!1})});