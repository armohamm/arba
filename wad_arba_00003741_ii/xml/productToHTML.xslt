<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="html" indent="yes"/>

    <xsl:template match="/">
      <html>
        <title> Products </title>
        <bod>
          <h1>Product List</h1>
          <table>
            <thead>
              <!--<th>Id</th>-->
              <th>Name</th>
              <th>Price</th>
              <th>Category</th>
            </thead>            
            <tbod>
              <xsl:for-each select="/Products/Product">
                <tr>
                  <td>
                    <xsl:value-of select="Id"/>
                  </td>
                  <td>
                    <xsl:value-of select="Name"/>
                  </td>
                  <td>
                    <xsl:value-of select="Price"/>
                  </td>
                  <td>
                    <xsl:value-of select="Category"/>
                  </td>
                </tr>
              </xsl:for-each>
            </tbod>
          </table>
        </bod>
      </html>
      
    </xsl:template>
</xsl:stylesheet>
