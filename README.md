This is a .net standard version of flickrnet

Be warned that I ran out of time to test this. It should, however, require minimal more effort to fix any bugs

I went off the master branch, went and changed every async method to the async/await  instead of the callback style. Once I changed WebCilent to System.Net.HttpClient all that bubbled up unless I wanted to do ugly hacks to preserve the old way.

(I noticed a sample that uses async/await later, so I'm thinking I should've gone off the v4 branch instead.)

Of course, that means I threw away backward compatibility.

Unit Tests were the most painful part. Nunit does not like the combination of desktop unit test with .net standard assembly.  Xunit does run, but that mean changing loads of test code.  That is why you will find a new FlickrNetTest-xUnit folder. You can switch between the two by renaming the folder you want to FlickrNetTest  (Incase Nunit gets their shit together, or someone wants to write .net core unit test projects, I left the original alone for the most part)

I haven't had time to fully test it since this took longer than I thought I need to get back to real work.

My intention is not to fork FlickrNet permanently, so I will not be taking pull requests or fixing issues. If you want real support for .net standard, please wait for the official FlickrNet to get there. If you need something that works for you now, this may be it.




