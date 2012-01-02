# "Networking for Web Devs", by Tatham Oddie

## Domain ideas

networkingforwebdevs.com

## Content Guidelines

Writing style:
* like hginit.com
* light-ish reading, easy words
* building story line
* something you read on its own like a short book (separate sections, pages, chapters, etc though)
* not something you'd use as a reference so much

Vendor agnostic (eg, it’ll tell you about A records, not how to create them in the Windows DNS MMC span-in)

Only enough implementation detail to explain how it works (eg, I wouldn't say anything about DNS generally running on UDP)

## Topics

* DNS
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
* DHCP
    * What it does, no more
* IPv4
    * Subnet masks and gateways
    * Pro topic / sidebar: your routing table
* IPv6
    * What it means for you
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

Out of scope:

* DNS
* glue records
* SRV records
* TCP internals (SYN, ACK, RST)
* BGP
* ARP

## How to contribute

[Fork it](http://hg.tath.am/nfwd).
