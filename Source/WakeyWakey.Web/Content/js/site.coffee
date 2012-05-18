$ ->
    #$("input[name='HostName']").change(() ->
    #    hostname = this.value
    #    return if hostname.length == 0
    #)

    # Setup MAC address lookup button
    $("#lookupmac").click(() ->
        hostname = $("input[name='HostName']").val()
        if (!hostname)
            alert("Please fill in a host name.")
            return

        $(this).attr("disabled", "disabled")
        $("#loader").fadeIn()
        $.get "/api/getmac/#{hostname}", (result) ->
            $("#loader").fadeOut()
            if (!result.ok)
                alert(result.message)
            else
                $("input[name='MacAddress']").val(result.message)
            $("#lookupmac").removeAttr("disabled")
    )

    # Setup remove buttons
    $("a.machine-remove").click((ev) ->
        ev.preventDefault()
        target = $(ev.currentTarget)

        if (!confirm("Really remove machine?"))
            return

        #target.button("loading")
        target.attr("disabled", "disabled")
        $.ajax this.href,
            type: 'DELETE'
            success: (result) ->
                #target.button("reset")
                row = target.closest("tr")
                row.children("td").each(() ->
                    $(this).wrapInner("<div></div>").children("div").slideUp("fast", "swing", () ->
                        row.hide()
                    )
                )
            error: (result) ->
                #target.button("reset")
                target.removeAttr("disabled")
                alert(result.message)
    )

    # Setup wake buttons
    $("a.machine-wake").click((ev) ->
        ev.preventDefault()
        target = $(ev.currentTarget)

        target.attr("disabled", "disabled")
        $.ajax this.href,
            type: 'POST'
            success: (result) ->
                alert("Wake up signal has been sent.")
                target.removeAttr("disabled")
            error: (result) ->
                alert(result.message)
                target.removeAttr("disabled")
    )
