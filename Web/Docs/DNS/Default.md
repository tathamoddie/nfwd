# DNS

At its simplest, DNS is often described as the phonebook of the internet. It converts names we remember, like `google.com`, to IP addresses we'd prefer to not even see, like `74.125.237.114` or `2404:6800:4006:800::1014`.

You'll also find DNS to play a critical part in delivering email, connecting VOIP calls, routing IMs and load balancing web traffic.

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

When you type `github.com` into your browser, this is almost exactly the process that your browser goes through.
