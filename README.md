# Vulnerability Analysis and Pen-Testing Suite #
## COMP2003-2020-S ##

## About The Project ##
This repository contains the relevant files for our COMP2003 - Computing Project. As Cyber Security students, we opted to create an application relevant to security.

## Product Vision ##
Designed for Small-to-Medium enterprises whose systems may be vulnerable to attacks without realising, the Vulnerability Analysis and Pen-Testing Suite is a system testing kit. It searches for a range of vulnerabilities using traditional tools wrapped into one, user-friendly environment that outputs any results clearly and effectively.

## Functionality ##
### ARP Scan ###
Our ARP (Address Resolution Protocol) viewer is a low-level network discovery tool, that gives users the ability to see the IP addresses and associated physical (MAC) addresses of devices on the network. In the screenshot below you can see VAPTS on the left giving the users a readout of a recent Arp scan, this can be compared to the default Arp command that ran through the command prompt on a windows machine on the right. You will notice that VAPTS can identify the devices manufacturer, due to our built-in web scraping techniques. We felt it was important for the users to be able to quickly identify the devices and having more detailed information about said devices allows for that to happen. 
![ARP Scan](https://github.com/creaseaj/COMP2003-2020-S/tree/main/Documentation/Screenshots/ArpScan.png "ARP Scan")

### Port Scan ###
Our port scanner allows the user to probe their network for open ports and gives a detailed readout of the port number, state, and description of what the port is used for, along with our recommendation on if the port should be open or closed.  This can be seen at the bottom in the screenshot below, with easy to see indicators on security risk and the number of ports within each of the three categories shown clearly. 
![Port Scan](https://github.com/creaseaj/COMP2003-2020-S/tree/main/Documentation/Screenshots/PortScan.png "Port Scan")


## Authors ##
* [Matt Caine](https://github.com/Matt-Caine)
* [Adam Crease](https://github.com/creaseaj)
* [Charlie Grayson](https://github.com/charlie-grayson)
* [Matt Hewitt](https://github.com/mhewitt9pq)
* [Kieran Wheatley](https://github.com/kieranwheatley)

## Acknowledgements ##
* [https://www.adminsub.net/mac-address-finder/](https://www.adminsub.net/mac-address-finder/)
* [https://nmap.org/](https://nmap.org/)
