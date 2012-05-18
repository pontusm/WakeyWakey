(function() {

  $(function() {
    $("#lookupmac").click(function() {
      var hostname;
      hostname = $("input[name='HostName']").val();
      if (!hostname) {
        alert("Please fill in a host name.");
        return;
      }
      $(this).attr("disabled", "disabled");
      $("#loader").fadeIn();
      return $.get("/api/getmac/" + hostname, function(result) {
        $("#loader").fadeOut();
        if (!result.ok) {
          alert(result.message);
        } else {
          $("input[name='MacAddress']").val(result.message);
        }
        return $("#lookupmac").removeAttr("disabled");
      });
    });
    $("a.machine-remove").click(function(ev) {
      var target;
      ev.preventDefault();
      target = $(ev.currentTarget);
      if (!confirm("Really remove machine?")) {
        return;
      }
      target.attr("disabled", "disabled");
      return $.ajax(this.href, {
        type: 'DELETE',
        success: function(result) {
          var row;
          row = target.closest("tr");
          return row.children("td").each(function() {
            return $(this).wrapInner("<div></div>").children("div").slideUp("fast", "swing", function() {
              return row.hide();
            });
          });
        },
        error: function(result) {
          target.removeAttr("disabled");
          return alert(result.message);
        }
      });
    });
    return $("a.machine-wake").click(function(ev) {
      var target;
      ev.preventDefault();
      target = $(ev.currentTarget);
      target.attr("disabled", "disabled");
      return $.ajax(this.href, {
        type: 'POST',
        success: function(result) {
          alert("Wake up signal has been sent.");
          return target.removeAttr("disabled");
        },
        error: function(result) {
          alert(result.message);
          return target.removeAttr("disabled");
        }
      });
    });
  });

}).call(this);
