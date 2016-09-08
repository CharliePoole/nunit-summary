<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:preserve-space elements="*" />
  <xsl:output omit-xml-declaration="yes" method="html" indent="yes" encoding="UTF-8" />
  <xsl:template match="/">
    <style type="text/css">
      * {
      font-family: "Times New Roman", Times, serif;
      }
      .strong {
      font-weight: bold;
      }
    </style>
    <xsl:apply-templates/>

  </xsl:template>

  <xsl:template match="test-run">

    <!-- Runtime Environment -->
    <div id="runtime">
      <span class="strong">Runtime Environment </span>

      <br />
      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;]]></xsl:text>
      <span class="strong">OS Version: </span>

      <span>
        <xsl:value-of select="test-suite/environment/@os-version[1]"/>
      </span>
      <br/>
      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;&nbsp;&nbsp;]]></xsl:text>
      <span class="strong"> CLR Version: </span>

      <span>
        <xsl:value-of select="@clr-version"/>
      </span>
      <br/>
      <br/>

      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
      <span class="strong">
        NUnit Version:
      </span>

      <span>
        <xsl:value-of select="@engine-version"/>
      </span>
      <br/>
    </div>
    <br/>

    <!-- Test Summary-->
    <div id="testsummary">
      <span class="strong">Test Run Summary</span>
      <br/>
      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
      <span class="strong"> Overall result: </span>
      <span>
        <xsl:value-of select="@result"/>
      </span>
      <br/>
      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>

      <span class="strong"> Test Count: </span>
      <span>
        <xsl:value-of select="@total"/>,
      </span>
      <span class="strong"> Passed: </span>
      <span>
        <xsl:value-of select="@passed"/>,
      </span>
      <span class="strong"> Failed: </span>
      <span>
        <xsl:value-of select="@failed"/>,
      </span>
      <span class="strong"> Inconclusive: </span>
      <span>
        <xsl:value-of select="@inconclusive"/>,
      </span>
      <span class="strong"> Skipped: </span>
      <span>
        <xsl:value-of select="@skipped"/>
      </span>
      <br/>

      <!-- [Optional] - Failed Test Summary -->
      <xsl:if test="@failed > 0">
        <xsl:variable name="failedTotal" select="count(//test-case[@result='Failed' and not(@label)])" />
        <xsl:variable name="errorsTotal" select="count(//test-case[@result='Failed' and @label='Error'])" />
        <xsl:variable name="invalidTotal" select="count(//test-case[@result='Failed' and @label='Invalid'])" />

        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;]]></xsl:text>
        <span class="strong">Failed Tests - Failures: </span>
        <span>
          <xsl:value-of select="$failedTotal"/>,
        </span>
        <span class="strong"> Errors: </span>
        <span>
          <xsl:value-of select="$errorsTotal"/>,
        </span>
        <span class="strong"> Invalid: </span>
        <span>
          <xsl:value-of select="$invalidTotal"/>
        </span>
        <br/>
      </xsl:if>

      <!-- [Optional] - Skipped Test Summary -->
      <xsl:if test="@skipped > 0">
        <xsl:variable name="ignoredTotal" select="count(//test-case[@result='Skipped' and @label='Ignored'])" />
        <xsl:variable name="explicitTotal" select="count(//test-case[@result='Skipped' and @label='Explicit'])" />
        <xsl:variable name="otherTotal" select="count(//test-case[@result='Skipped' and not(@label='Explicit' or @label='Ignored')])" />
      
        <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;]]></xsl:text>
        <span class="strong">Skipped Tests - Ignored: </span>
        <span>
          <xsl:value-of select="$ignoredTotal"/>,
        </span>
        <span class="strong"> Explicit: </span>
        <span>
          <xsl:value-of select="$explicitTotal"/>,
        </span>
        <span class="strong"> Other: </span>
        <span>
          <xsl:value-of select="$otherTotal"/>
        </span>

        <br/>
      </xsl:if>

      <!-- Times -->
      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;&nbsp;]]></xsl:text>
      <span class="strong">Start time: </span>
      <span>
        <xsl:value-of select="@start-time"/>
      </span>
      <br/>
      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;&nbsp;&nbsp;&nbsp]]></xsl:text>
      <span class="strong">End time: </span>
      <span>
        <xsl:value-of select="@end-time"/>
      </span>
      <br/>
      <xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;&nbsp;&nbsp;&nbsp;]]></xsl:text>
      <span class="strong">Duration: </span>
      <span>
        <xsl:value-of select="format-number(@duration,'0.000')"/> seconds
      </span>
      <br/>
    </div>
   

    <br/>

  </xsl:template>

</xsl:stylesheet>
