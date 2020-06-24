$(document).ready(function(){

				$('#owl-demos').owlCarousel({
					autoplay: false,
					loop: true,
					items:4,
					margin:30,
					stagePadding:30,
					smartSpeed:400,
					nav: true,
					dots: false,
					responsiveClass:true,
					responsive:{
						0:{
							items:1,
						},
						576:{
							items:2,
						},
						991:{
							items:3,
						},
						1200:{
							items:4,
						},
					}
				});
				
				$('.menu-link').bigSlide();
				
				$('#a').click(function(e){
			        e.preventDefault();
			        $('#a1,#e1,#d,#b,#c').show(500);
					$('#b1,#c1,#d1,#a,#e2,#e3,#e4').hide(500);
		        });
				
				$('#b').click(function(e){
			        e.preventDefault();
			        $('#b1,#e2,#a,#d,#c').show(500);
					$('#a1,#c1,#d1,#b,#e1,#e3,#e4').hide(500);
		        });
				
				$('#c').click(function(e){
			        e.preventDefault();
			        $('#c1,#e3,#a,#b,#c').show(500);
					$('#a1,#b1,#d1,#c,#e2,#e1,#e4').hide(500);
		        });
				
				$('#d').click(function(e){
			        e.preventDefault();
			        $('#d1,#e4,#a,#b,#c').show(500);
					$('#a1,#c1,#b1,#d,#e2,#e3,#e1').hide(500);
		        });
				
				$('#e1').click(function(e){
			        e.preventDefault();
			        $('#a,#b,#c,#d').show(200);
					$('#b1,#c1,#d1,#a1,#e1,#e2,#e3,#e4').hide(500);
		        });
				$('#e2').click(function(e){
			        e.preventDefault();
			        $('#a,#b,#c,#d').show(200);
					$('#b1,#c1,#d1,#a1,#e1,#e2,#e3,#e4').hide(500);
		        });
				$('#e3').click(function(e){
			        e.preventDefault();
			        $('#a,#b,#c,#d').show(200);
					$('#b1,#c1,#d1,#a1,#e1,#e2,#e3,#e4').hide(500);
		        });
				$('#e4').click(function(e){
			        e.preventDefault();
			        $('#a,#b,#c,#d').show(200);
					$('#b1,#c1,#d1,#a1,#e1,#e2,#e3,#e4').hide(500);
		        });
				
				
				$(function() {
				  $('.skitter-large').skitter({
					dots: false,
					progressbar: true,
					interval: 7500,
					fullscreen: true,
					navigation: true
				  });
				  
				});
				
});