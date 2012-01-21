# Networking for Web Devs

In this crazy world of cloud computing, developers are dealing with more and more infrastructure tasks. You create a GitHub repo, spin up a Heroku app, push to it, then ... DNS? Huh? What if you want to support URLs like http://*username*.example.com on Windows Azure?

The goal of this site is to give developers enough infrastrucute knowledge to feel comfortable in this new era, without making them sys-admins.

## Domain Ideas

networkingforwebdevs.com

## Content Guidelines

Writing style:

* like [hginit.com](http://hginit.com)
* light-ish reading, easy words
* building story line
* something you read on its own like a short book (separate sections, pages, chapters, etc though)
* not something you'd use as a reference so much

Vendor agnostic (eg, we’ll tell you about A records, but not how to create them in the Windows DNS MMC span-in)

Only enough implementation detail to explain how it works (eg, we wouldn't say anything about DNS generally running on UDP)

## Topics

* [DNS](dns)
    * What DNS does (names to IPs)
    * What DNS does not do (names to port numbers)
    * A and AAAA records
    * Multiple A records
        * Round robin
    * Wildcards
    * CNAME records
    * MX records
    * NS records
    * SOA and TTL
    * Recursion
    * Reserved TLDs ([RFC 2606](http://tools.ietf.org/rfc/rfc2606.txt))
* IPv4
    * Subnet masks and gateways
    * Pro topic / sidebar: your routing table
* IPv6
    * What it means for you
* DHCP
    * What it does, no more
* HTTP
    * Bindings apply to a port, on an IP, on an adapter
    * How host headers work
    * Reverse proxy architectures to expose multiple services on one port (eg, republish TeamCity through IIS)
    * Response semantics
        * 401
        * 301 vs 302
    * URLs (not really sure if this is in scope for a networking primer?)
        * Scheme-relative but with a different host
        * Other schemes that are HTTP but with different semantics (eg webcal)
        * Host is case-insensitive but path is case-sensitive
        * Query strings and hashes
* How to diagnose a connection issue
    * Don’t just type mysite.com into a browser, or just ping it
    * Name resolution -> telnet to the port -> etc
    * Distinction between name resolution and ICMP ping
* Certificates
    * Private vs public key and PKI in general
    * HTTPS limitations of one cert per IP+Port unless you’re lucky to have Server Name Indication support on server and client
    * Wildcard certs and Subject Name Alternatives to work around IP+Port limits
 * Useful recipes
     * One IP, multiple sites
     * One site, multiple host names
     * One server, multiple SSL sites

Out of scope:

* DNS
    * glue records
    * SRV records
* TCP internals (SYN, ACK, RST)
* BGP
* ARP

## Contributors

* [Tatham Oddie](http://tath.am)

## How to Contribute

Click the _Edit this page_ link at the top of any page, or [fork the entire repository](https://github.com/tathamoddie/nfwd).
