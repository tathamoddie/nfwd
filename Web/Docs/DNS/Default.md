# DNS

At its simplest, DNS is often described as the phonebook of the internet. It converts names we remember, like `google.com`, to IP addresses we'd prefer to not even see, like `74.125.237.114` or `2404:6800:4006:800::1014`.

You'll also find DNS to play a critical role in delivering email, connecting VOIP calls, routing IMs and load balancing web traffic.

Generally, it's the last piece of the deployment puzzle before you can finally share your creation with the world. (Unless you want to tell people about your cool new site, `74.125.237.114`.)

## Your First Query

To query DNS, there's a useful command line tool on both Windows and UNIX called `nslookup`. Launching it without any arguments results in an interactive prompt of its own:

    C:\> nslookup
    Default Server:  router
    Address:  10.0.0.1
    
    >

Try typing `github.com` at this new prompt and you'll see something like this:

    > github.com
    Server:  router
    Address:  10.0.0.1
    
    Non-authoritative answer:
    Name:    github.com
    Address:  207.97.227.239

As easy as that, you've now found the IP address of GitHub's web servers: `207.97.227.239`.

When you type `github.com` into your browser, it uses a similar process to determine where to connect to.

## Record Types

In our first query, we retrieved something called an *A record*. This is the simplest record of DNS, allowing us to define an address for a hostname.

You've now mastered the idea of DNS-as-a-phonebook, so lets look a bit deeper: DNS doesn't always return addresses. There are a number of different record types, each with a defined structure.

### Mail Exchangers (MX)

If we were trying to deliver email to GitHub, we'd need to ask for their *MX (mail exchange) record* instead.

Configure `nslookup` to query for MX records:

    > set type=MX

Then, run another query for `github.com`:
    
    > github.com
    Server:  router
    Address:  10.0.0.1
    
    Non-authoritative answer:
    github.com      MX preference = 10, mail exchanger = ALT2.ASPMX.L.GOOGLE.com
    github.com      MX preference = 10, mail exchanger = ASPMX.L.GOOGLE.com
    github.com      MX preference = 10, mail exchanger = ASPMX2.GOOGLEMAIL.com
    github.com      MX preference = 10, mail exchanger = ASPMX3.GOOGLEMAIL.com
    github.com      MX preference = 10, mail exchanger = ALT1.ASPMX.L.GOOGLE.com

Here we can see that there are five different hostnames that GitHub are happy for you to deliver email to, each of equal priority.

Let's not dwell on mail servers for too long though. Any mail provider you choose will give you clear instructions about what records you'll need in place.

### Canonical Names (CNAME)

As a web developer, it's likely that you've seen or heard the term *CNAME* before. CNAME is short for *canonical name*. It works like a shortcut, allowing us to alias one hostname to another.

Try this lookup:

    > set type=CNAME
    > longitude.tath.am
    Server:  router
    Address:  10.0.0.1
    
    Non-authoritative answer:
    longitude.tath.am       canonical name = longitude.heroku.com

In this case, DNS is telling us that `longitude.tath.am` is actually `longitude.heroku.com`.

The big advantage here is that when Heroku decide to change the IP address for my app, everything will still work. If I used an A record instead, I'd be taking a bet against Heroku's infrastructure ever changing.

It's not all peachy though; there are some major restrictions applied to CNAME records. When a CNAME is defined, it must be the only record for that hostname. Because there are certain other records which are required at the root of a zone, CNAMEs are effectively banned there. This means you could CNAME `www.example.com` to `example.heroku.com`, but not `example.com` itself.

For this reason, services like Heroku and AppHarbor also publish a set of known IPs which are unlikely to change. Wherever possible in these arrangements, you should try and use a CNAME record. Only use A records at the root, or where you are also managing the IP addresses and thus will know if they ever need to change.

Of course, if you're still managing IP addresses, you probably want to take a moment to step back and ask yourself why you're clutching on to the '90s so tightly. Owning your own hardware? How quaint.

## Recursion

<aside>
Prior to DNS being invented, a single computer was used to host a file called `HOSTS.TXT`. This contained *every* hostname on the internet, with updates distributed via FTP.
</aside>

## Round Robin
