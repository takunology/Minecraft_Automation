server_address = "localhost"
server_pass = "minecraft"

from mcrcon import MCRcon

with MCRcon(server_address, server_pass, 25575)as mcr:
    mcr.command("/time set 0")
    mcr.command("/weather clear")